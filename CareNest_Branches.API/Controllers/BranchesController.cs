
using CareNest_Branches.Application.Common;
using CareNest_Branches.Application.Features.Commands.Create;
using CareNest_Branches.Application.Features.Commands.Delete;
using CareNest_Branches.Application.Features.Commands.Update;
using CareNest_Branches.Application.Features.Queries.GetAllPaging;
using CareNest_Branches.Application.Features.Queries.GetById;
using CareNest_Branches.Application.Interfaces.CQRS;
using CareNest_Branches.Domain.Commons.Constant;
using CareNest_Branches.Domain.Entitites;
using CareNest_Branches.Extensions;
using Microsoft.AspNetCore.Mvc;


namespace CareNest_Branches.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchesController : ControllerBase
    {
        private readonly IUseCaseDispatcher _dispatcher;

        public BranchesController(IUseCaseDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        /// <summary>
        /// Hiển thị toàn bộ danh sách chi nhánh  hiện có trong hệ thống với phân trang và sắp xếp
        /// </summary>
        /// <param name="pageIndex">trang hiện tại</param>
        /// <param name="pageSize">Số lượng phần tử trong trang</param>
        /// <param name="sortColumn">cột muốn sort: name, updateat,ownerid</param>
        /// <param name="sortDirection">cách sort asc or desc</param>
        /// <returns>Danh sách chi nhánh </returns>
        [HttpGet]
        public async Task<IActionResult> GetPaging(
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? sortColumn = null,
            [FromQuery] string? sortDirection = "asc",
            [FromQuery] string? shopId = null)
        {
            var query = new GetAllPagingQuery()
            {
                Index = pageIndex,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortDirection = sortDirection,
                ShopId = shopId
            };
            var result = await _dispatcher.DispatchQueryAsync<GetAllPagingQuery, PageResult<BranchesResponse>>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// Hiển thị chi tiết chi nhánh  theo id
        /// </summary>
        /// <param name="id">Id chi nhánh </param>
        /// <returns>chi tiết chi nhánh </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetByIdQuery() { Id = id };
            Branches result = await _dispatcher.DispatchQueryAsync<GetByIdQuery, Branches>(query);
            return this.OkResponse(result, MessageConstant.SuccessGet);
        }

        /// <summary>
        /// tạo mới chi nhánh 
        /// </summary>
        /// <param name="command">thông tin chi nhánh </param>
        /// <returns>thông tin chi nhánh  mới tạo xog</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCommand command)
        {
            Branches result = await _dispatcher.DispatchAsync<CreateCommand, Branches>(command);

            return this.OkResponse(result, MessageConstant.SuccessCreate);
        }

        /// <summary>
        /// Cập nhật thông tin chi nhánh 
        /// </summary>
        /// <param name="id">Id chi nhánh </param>
        /// <param name="request">các thông tin cần sửa</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateRequest request)
        {

            var command = new UpdateCommand()
            {
                Id = id,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                ShopId = request.ShopId,
            };
            Branches result = await _dispatcher.DispatchAsync<UpdateCommand, Branches>(command);

            return this.OkResponse(result, MessageConstant.SuccessUpdate);
        }

        /// <summary>
        /// xoá chi nhánh 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _dispatcher.DispatchAsync(new DeleteCommand { Id = id });
            return this.OkResponse(MessageConstant.SuccessDelete);
        }
    }
}
