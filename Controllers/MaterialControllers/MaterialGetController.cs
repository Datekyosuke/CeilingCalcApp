using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.MaterialControllers
{
    [Route("/api/MaterailController")]
    [ApiController]
    public class MaterialGetController : Controller
    {
        private IMaterialRepository _materialRepository;
        private readonly IUriService uriService;

        public MaterialGetController(IMaterialRepository materialRepository, IUriService uriService)
        {
            _materialRepository = materialRepository;
            this.uriService = uriService;
        }
        /// <summary>
        /// Returns a paginated, sorted and ranged list of materials. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of materials.
        /// 
        /// Properties can take Material field values:
        /// 
        ///     Id
        ///     Texture
        ///     Color
        ///     Size
        ///     Price
        /// Sort is enum
        /// 
        ///     0 ASc
        ///     1 Desc
        ///     null without sorting
        ///     
        ///  Min  - search for debts from
        /// 
        ///  Max - search for debts up
        /// </remarks>
        /// <returns>Page list materials</returns>
        /// <response code="200">materials retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var totalRecords = _materialRepository.Count();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, totalRecords);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var entities = _materialRepository.GetAll(validFilter, expression, sort, ranges);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Material>(entities, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }
    }
}
