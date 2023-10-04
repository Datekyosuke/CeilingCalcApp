using Microsoft.AspNetCore.JsonPatch;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        public Task Delete(TEntity entity);
        public Task Put(TEntity oldEntity, TEntity entity);
        public Task JsonPatchWithModelState(TEntity entity,
         JsonPatchDocument<TEntity> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(TEntity entity);
        public Task<TEntity> GetAsync(int id);
        public Task Patch(TEntity oldEntity, TEntity entity);
        public Task<PagedResponse<List<TEntity>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
