using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Repository;

namespace WebApiDB.Controllers.MaterialControllers
{
    [Route("/api/MaterailController")]
    [ApiController]
    public partial class MaterialController : Controller
    {
        private IMaterialRepository _materialRepository;
        private readonly IUriService _uriService;
        private IValidator<Material> _validatorMaterial;

        public MaterialController(IMaterialRepository materialRepository, IUriService uriService, IValidator<Material> validatorMaterial)
        {
            _materialRepository = materialRepository;
            _uriService = uriService;
            _validatorMaterial = validatorMaterial;
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
        ///  Min  - search for price from
        /// 
        ///  Max - search for price up
        /// </remarks>
        /// <returns>Page list materials</returns>
        /// <response code="200">materials retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public virtual IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var trimSearchString = searchString?.Trim();
            var pagedReponse = _materialRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString, route).Result;
            return Ok(pagedReponse);
        }
    }
}
