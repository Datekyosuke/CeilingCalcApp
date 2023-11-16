using CeilingCalc.Models;
using WebApiDB.Helpers;
using WebApiDB.Pagination;

namespace CeilingCalc.Interfaces
{
    public interface IOrderDetailRepository
    {
        public Task<OrderDetail> GetAsync(int id);
        public Task Delete(OrderDetail orderDetail);
        public Task Put(OrderDetail oldOrderDetail, OrderDetail orderDetail);
        public Task JsonPatchWithModelState();
        public Task Post(OrderDetail orderDetail);

        public Task Patch(OrderDetail oldOrderDetail, OrderDetail orderDetail);

        public Task<PagedResponse<List<OrderDetail>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
