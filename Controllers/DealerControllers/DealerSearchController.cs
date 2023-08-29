using Microsoft.AspNetCore.Mvc;
using SoftWx.Match;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using static WebApiDB.Helpers.LevenshteinDistance;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerSearchController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerSearchController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }
        /// <summary>
        /// Returns dealer by Id
        /// </summary>
        /// <param name="searchString"> Строка для поиска</param>
        /// <returns>Dealer</returns>
        /// <response code="200">Dealer retrieved</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't lookup your Dealer right now</response>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(Dealer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get([FromQuery] string searchString)
        {
            var dealers = _dealerRepository.GetAll();
            var matches = new List<Dealer>();
            foreach (var dealer in dealers)
            {
                if (Distance(dealer.LastName, searchString) <= 2)
                { matches.Add(dealer); continue; }
                if (Distance(dealer.FirstName, searchString) <= 2)
                { matches.Add(dealer); continue; }
                if (Distance(dealer.City, searchString) <= 2)
                { matches.Add(dealer); continue; }

            }
            return Ok(matches);
        }
    }
}
