using CeilingCalc.Data.DTO_Material;
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
        public Task JsonPatchWithModelState();
        public Task Post(Material material);

        public Task Patch(Material oldMaterial, Material material);

        public Task<PagedResponse<List<MaterialDTO>>> GetAllAsync(PaginationFilter validFilter, string expression, string sort, NumericRanges ranges, string searchString, string? route);
    }
}
