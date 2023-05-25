using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    public class DealerConytoller : Controller
    {
       DealerContext db = new DealerContext();

      

        [Route("/api/[controller]")]
  
        [HttpGet()]    
        /// <summary>
        /// Кeturns a list of all dealers
        /// </summary>
        /// <returns>list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        public IEnumerable<Dealer> Get() => db.Dealers;
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
            var dealer = db.Dealers.SingleOrDefault(p => p.Id == id);

            if (dealer != null)
            {
                db.Dealers.Remove(db.Dealers.SingleOrDefault(p => p.Id == id));
                await db.SaveChangesAsync();
            }
            else return NotFound();
            return Ok();
        }
     
        [HttpPost]
        public async Task<IActionResult> Post(Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.Entry(dealer).State = EntityState.Added;
            await db.SaveChangesAsync();
            return Ok();
        }
        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>

        [HttpPost("AddDealer")]
        public Task<IActionResult> PostBody([FromBody] Dealer dealer) =>
            Post(dealer);
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
        /// <summary>
        /// Making changes to one or more dealer fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "FirstName",
        ///        "value": "Barry"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the FirstName field of the selected dealer by id to "Barry"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        ///     Fields dealer:
        ///     
        ///     "Id": can't be changed, integer
        ///     "FirstName": "string", required
        ///     "LastName": "string", may be null
        ///     "Telephone": 100000000000, integer, must be between 1 and 100000000000
        ///     "Debts": 0, integer
        ///     "City": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("{id}")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
             [FromBody] JsonPatchDocument<Dealer> patchDoc)
        {
            if (patchDoc != null)
            {
                var customer = db.Dealers.SingleOrDefault(p => p.Id == id);

                patchDoc.ApplyTo(customer, ModelState);
                await db.SaveChangesAsync();
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

    }
}
