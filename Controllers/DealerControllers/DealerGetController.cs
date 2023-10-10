using FuzzySharp;
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
    [ApiController]
    public partial class DealerController : Controller
    {

        private IDealerRepository _dealerRepository;
        private readonly IUriService _uriService;

        public DealerController(IDealerRepository dealerRepository, IUriService uriService)
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
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required    
        /// Sort 
        /// 
        ///     asc - ascending
        ///     desc - descending
        ///     else without sorting
        ///     
        ///  min  - search for debts from
        /// 
        ///  max - search for debts up
        /// </remarks>
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public virtual IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum"); 
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var sortDealers = _dealerRepository.Get(expression, sort).Result;
            string[] propertySearch = { "LastName", "FirstName", "City" };
            var matches = SearchHelper.Search(sortDealers.ToList(), searchString, propertySearch).Distinct();
            var totalRecords = matches.Count();
            var sortedSearchEntities = matches
                   .Where(x => x.Debts >= ranges.Min && x.Debts <= ranges.Max)
                   .Distinct()
                   .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                   .Take(validFilter.PageSize)
                   .ToList();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Dealer>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            

            
            return Ok(pagedReponse);
        }

    }
}
