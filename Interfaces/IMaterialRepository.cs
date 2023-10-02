using Microsoft.AspNetCore.JsonPatch;
using WebApiDB.Helpers;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Interfaces
{
    public interface IMaterialRepository
    {
        public List<Material> GetAll();
        public Task Delete(Material materail);
        public Task Put(Material oldMaterial, Material material);
        public Task JsonPatchWithModelState(Material material,
         JsonPatchDocument<Material> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelStat);
        public Task Post(Material material);
        public Material Get(int id);

        public Task Patch(Material oldMaterial, Material material);

        public int Count();
        public List<Material> GetAll(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges);
    }
}
