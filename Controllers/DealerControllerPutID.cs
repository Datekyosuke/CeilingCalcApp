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
            db.Entry(oldClient).CurrentValues.SetValues(dealer);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}
