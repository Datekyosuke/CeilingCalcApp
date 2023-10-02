using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.DealerControllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/DealerController")]
    public class DealerGetController : Controller
    {

        private IDealerRepository _dealerRepository;
        private readonly IUriService _uriService;

        public DealerGetController(IDealerRepository dealerRepository, IUriService uriService)
        {
            _dealerRepository = dealerRepository;
            _uriService = uriService;
        }
        /// <summary>
        /// Returns a paginated, sorted and ranged list of dealers. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of dealers.
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     Id
        ///     FirstName
        ///     LastName
        ///     Telephone
        ///     Debts
        ///     City
        ///     
        /// Sort 
        /// 
        ///     Asc - ascending
        ///     Desc - descending
        ///     else without sorting
        ///     
        ///  Min  - search for debts from
        /// 
        ///  Max - search for debts up
        /// </remarks>
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum"); 
            var route = Request.Path.Value;
            var totalRecords = _dealerRepository.Count();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, totalRecords);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var entities = _dealerRepository.GetAllAsync(validFilter, expression, sort, ranges).Result;
            var pagedReponse = PaginationHelper.CreatePagedReponse<Dealer>(entities, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

    }
}
