using Microsoft.AspNetCore.JsonPatch;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IDealerRepository
    {
        public Task<Dealer> GetAsync(int id);
        public Task Delete(Dealer dealer);
        public Task Put(Dealer oldClient, Dealer dealer);
        public Task JsonPatchWithModelState(Dealer dealer,
         JsonPatchDocument<Dealer> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Dealer dealer);

        public Task Patch(Dealer oldClient, Dealer dealer);

        public Task<PagedResponse<List<Dealer>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
