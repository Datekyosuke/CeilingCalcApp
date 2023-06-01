using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerControllerPutID : ControllerBase
    {
        DealerContext db = new DealerContext();
        /// <summary>
        /// Making changes to one dealer record of a specific ID
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Dealer dealer)
        {
            var oldClient = await db.Dealers.FindAsync(id);
            if (oldClient == null)
                return NotFound();

            if (dealer.FirstName.Length > 50)
                return BadRequest("FirstName cannot be more than 50 characters");
            if (dealer.Telephone < 10000000000 || dealer.Telephone > 99999999999)
            {
                return BadRequest("Invalid phone number. Must contain 10 digits!");
            }
            if (dealer.LastName.Length > 50 || dealer.LastName.Length < 2)
                return BadRequest("LastName cannot be more than 50 and less than 2 characters");
            if (dealer.Debts > float.MaxValue || dealer.Debts < float.MinValue)
                return BadRequest("Wrong debts! Too big (small) number");
            if (dealer.City.Length > 50 || dealer.City.Length < 2)
                return BadRequest("City cannot be more than 50 and less than 2 characters");

            db.Entry(oldClient).CurrentValues.SetValues(dealer);
            await db.SaveChangesAsync();
            return Ok("Dealer changed!");
        }
    }
}
