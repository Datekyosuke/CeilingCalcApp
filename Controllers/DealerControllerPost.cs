using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerControllerPost : ControllerBase
    {
        DealerContext db = new DealerContext();

        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost("AddDealer")]
        public async Task<IActionResult> Post([FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(dealer).State = EntityState.Added;
            await db.SaveChangesAsync();
            return Ok();
        }      

    }
}
