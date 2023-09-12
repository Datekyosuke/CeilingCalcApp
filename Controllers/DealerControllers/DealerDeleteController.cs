using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerDeleteController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerDeleteController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }
        /// <summary>
        /// Removes dealer by id
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Dealer removed</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't remove your Dealer right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dealer = _dealerRepository.Get(id);

            if (dealer != null)
            {
                _dealerRepository.Delete(dealer);
            }
            else return NotFound();
            return Ok("Dealer deleted!");
        }
    }
}
