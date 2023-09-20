using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerPostController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerPostController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dealer dealer)
        {
            var validation = ValidationDealer.DealerValidation(dealer);
            if (!validation.Item1)
            {
                return BadRequest(validation.Item2);
            }
            
            await _dealerRepository.Post(dealer);
            return Ok("Dealer created!");
        }

    }
}
