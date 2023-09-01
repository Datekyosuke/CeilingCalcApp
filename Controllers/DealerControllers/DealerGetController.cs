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
        /// </remarks>
        /// <returns>Page list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        [HttpGet("Pagination")]
        public IActionResult GetAll([FromQuery] PaginationFilter filter)
        {
            if(filter.PageSize <= 0)
            {
                return BadRequest("Page size must be greater than 0");
            }
            var route = Request.Path.Value;
            var totalRecords = _dealerRepository.Count();
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize, totalRecords);
            var entities =  _dealerRepository.GetAll(validFilter);
            var pagedReponse = PaginationHelper.CreatePagedReponse<Dealer>(entities, validFilter, totalRecords, uriService, route);
            return Ok(pagedReponse);
        }

    }
}
