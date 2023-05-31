using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerControllerPatch : ControllerBase
    {
        DealerContext db = new DealerContext();
        /// <summary>
        /// Making changes to one dealer record of a specific ID. Do not work yet
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "Id": can't be changed, integer
        ///     "FirstName": "string", required
        ///     "LastName": "string", may be null
        ///     "Telephone": 100000000000, integer, must be between 1 and 100000000000
        ///     "Debts": 0, integer
        ///     "City": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPatch("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] Dealer dealer)
        {
            var oldClient = await db.Dealers.FindAsync(id);
            if (oldClient == null)
                return NotFound();
            if(dealer.FirstName == "string")
                dealer.FirstName = oldClient.FirstName;
            if (dealer.LastName == "string")
                dealer.LastName = oldClient.LastName;
            if(dealer.Telephone == 100000000000)
                dealer.Telephone = oldClient.Telephone;
            if (dealer.Debts == 0)
                dealer.Debts = oldClient.Debts;
            if (dealer.City == "string")
                dealer.City = oldClient.City;
            db.Entry(oldClient).CurrentValues.SetValues(dealer);
            await db.SaveChangesAsync();
            return Ok();
        }
    }
}