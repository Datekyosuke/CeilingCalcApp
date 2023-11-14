using CeilingCalc.Data.DTO_Material;
using Microsoft.AspNetCore.JsonPatch;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IMaterialRepository
    {
        public  Task<Material> GetAsync(int id);
        public Task Delete(Material material);
        public Task Put(Material oldMaterial, Material material);
        public Task JsonPatchWithModelState(Material material,
         JsonPatchDocument<Material> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Material material);

        public Task Patch(Material oldMaterial, Material material);

        public Task<PagedResponse<List<MaterialDTO>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
