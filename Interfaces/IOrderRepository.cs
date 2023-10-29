using Microsoft.AspNetCore.JsonPatch;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IOrderRepository
    {
        public Task<Order> GetAsync(int id);
        public Task Delete(Order order);
        public Task Put(Order oldOrder, Order order);
        public Task JsonPatchWithModelState(Order order,
         JsonPatchDocument<Order> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Order order);

        public Task Patch(Order oldOrder, Order order);

        public Task<PagedResponse<List<OrderG>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
