using CareNest_Branches.API.Middleware;
using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.Features.Commands.Create;
using CareNest_Branches.Application.Features.Commands.Delete;
using CareNest_Branches.Application.Features.Commands.Update;
using CareNest_Branches.Application.Features.Queries.GetAllPaging;
using CareNest_Branches.Application.Features.Queries.GetById;
using CareNest_Branches.Application.Interfaces.CQRS;
using CareNest_Branches.Application.Interfaces.CQRS.Commands;
using CareNest_Branches.Application.Interfaces.CQRS.Queries;
using CareNest_Branches.Application.Interfaces.Services;
using CareNest_Branches.Application.Interfaces.UOW;
using CareNest_Branches.Application.UseCases;
using CareNest_Branches.Domain.Entitites;
using CareNest_Branches.Domain.Repositories;
using CareNest_Branches.Infrastructure.Persistences.Configuration;
using CareNest_Branches.Infrastructure.Persistences.Database;
using CareNest_Branches.Infrastructure.Persistences.Repository;
using CareNest_Branches.Infrastructure.Services;
using CareNest_Branches.Infrastructure.UOW;
using CareNest_Branchesry.Application.Common.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
// Lấy DatabaseSettings từ configuration
DatabaseSettings dbSettings = builder.Configuration.GetSection("DatabaseSettings").Get<DatabaseSettings>()!;
dbSettings.Display();
string connectionString = dbSettings!.GetConnectionString();


// Đăng ký DbContext với PostgreSQL Pooling + Timeout phục vụ cho Koyeb
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseNpgsql(
        connectionString + ";Pooling=true;Maximum Pool Size=5;Minimum Pool Size=0;Timeout=15;",
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(5),
                errorCodesToAdd: null);
            // Có thể nâng timeout nếu cần
            // npgsqlOptions.CommandTimeout(60);
        }));

builder.Services.AddTransient<DatabaseSeeder>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Đăng ký service thêm chú thích cho api
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    //ADD JWT BEARER SECURITY DEFINITION
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập token theo định dạng: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        //Type = SecuritySchemeType.ApiKey,
        Type = SecuritySchemeType.Http,//ko cần thêm token phía trước
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                In = ParameterLocation.Header,
                Name = "Bearer",
                Scheme = "Bearer"
            },
            new List<string>()
        }
    });
});

// Đăng ký các repository
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//command
builder.Services.AddScoped<ICommandHandler<CreateCommand, Branches>, CreateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateCommand, Branches>, UpdateCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteCommand>, DeleteCommandHandler>();
//query
builder.Services.AddScoped<IQueryHandler<GetAllPagingQuery, PageResult<BranchesResponse>>, GetAllPagingQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetByIdQuery, Branches>, GetByIdQueryHandler>();

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings")
);



// Đăng ký cấu hình APIServiceOption
builder.Services.Configure<APIServiceOption>(
    builder.Configuration.GetSection("APIService")
);
//Đăng ký lấy thông tin từ token
builder.Services.AddHttpClient<IAPIService, APIService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IAPIService, APIService>();
builder.Services.AddScoped<IShopService, ShopService>();


builder.Services.AddScoped<IUseCaseDispatcher, UseCaseDispatcher>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
        });
});



var app = builder.Build();

// Cho phép bật Swagger ở Development hoặc khi cấu hình Swagger:Enabled=true
var swaggerEnabled = app.Environment.IsDevelopment() || builder.Configuration.GetValue<bool>("Swagger:Enabled");
if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Chỉ migrate khi RUN_MIGRATIONS=true (ENV) để phù hợp chuẩn Koyeb
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    var runMigrations = Environment.GetEnvironmentVariable("RUN_MIGRATIONS");
    if (!string.IsNullOrWhiteSpace(runMigrations) && runMigrations.Equals("true", StringComparison.OrdinalIgnoreCase))
    {
        context.Database.Migrate();
    }
}
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();