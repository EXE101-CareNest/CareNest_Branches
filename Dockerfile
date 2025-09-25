# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CareNest_Branches.API/CareNest_Branches.API.csproj", "CareNest_Branches.API/"]
COPY ["CareNest_Branches.Application/CareNest_Branches.Application.csproj", "CareNest_Branches.Application/"]
COPY ["CareNest_Branches.Domain/CareNest_Branches.Domain.csproj", "CareNest_Branches.Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["CareNest_Branches.Infrastructure/CareNest_Branches.Infrastructure.csproj", "CareNest_Branches.Infrastructure/"]
RUN dotnet restore "./CareNest_Branches.API/CareNest_Branches.API.csproj"
COPY . .
WORKDIR "/src/CareNest_Branches.API"
RUN dotnet build "./CareNest_Branches.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CareNest_Branches.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CareNest_Branches.API.dll"]