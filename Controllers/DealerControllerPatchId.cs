using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerControllerPatchId : ControllerBase
    {
        DealerContext db = new DealerContext();
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
