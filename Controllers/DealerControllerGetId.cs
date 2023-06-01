using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerControllerGetId : ControllerBase
    {
        DealerContext db = new DealerContext();
        /// <summary>
        /// Returns dealer by Id
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <returns>Dealer</returns>
        /// <response code="200">Dealer retrieved</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't lookup your Dealer right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Dealer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var dealer = db.Dealers.SingleOrDefault(p => p.Id == id);

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(dealer);
        }
    }
}
