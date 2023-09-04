using Castle.Core.Resource;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Helpers;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;

namespace WebApiDB.Controllers.DealerControllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/DealerController")]
    public class DealerGetController : Controller
    {

        private IDealerRepository _dealerRepository;
        private readonly IUriService uriService;

        public DealerGetController(IDealerRepository dealerRepository, IUriService uriService)
        {
            _dealerRepository = dealerRepository;
            this.uriService = uriService;
        }
        /// <summary>
        /// Returns a paginated list of dealers. 
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
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public IActionResult GetAllSort([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum"); 
            var route = Request.Path.Value;
            var totalRecords = _dealerRepository.Count();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, totalRecords);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var entities = _dealerRepository.GetAllSort(validFilter, expression, sort, ranges);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Dealer>(entities, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

    }
}
