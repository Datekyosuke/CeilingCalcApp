using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerPatchJSONController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerPatchJSONController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
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
        ///     "FirstName": "string", may be null
        ///     "LastName": "string", required
        ///     "Telephone": 11 digit, integer
        ///     "Debts": 0, integer
        ///     "City": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("PatchJson")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<Dealer> patchDoc)
        {

            if (patchDoc.Operations[0].path == "LastName" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("LastName cannot be more than 50 and less than 2 characters");

            if (patchDoc.Operations[0].path == "FirstName" && patchDoc.Operations[0].value.ToString().Length > 50)
                return BadRequest("FirstName cannot be more than 50 characters");

            if (patchDoc.Operations[0].path == "Telephone")
            {
                long.TryParse(patchDoc.Operations[0].value.ToString(), out long telephone);
                if(telephone < 10000000000 || telephone > 99999999999)
                 return BadRequest("Invalid telephone. Must contain 11 digits!");
            }

            if (patchDoc.Operations[0].path == "Debts")
            {
                {
                    if(!float.TryParse(patchDoc.Operations[0].value.ToString(), out float debts))
                        return BadRequest("Wrong debts! Must be a number");
                    else if (debts < float.MinValue || debts > float.MaxValue)
                        return BadRequest("Wrong debts! Too big (small) number");
                }

            }
            if (patchDoc.Operations[0].path == "City" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("City cannot be more than 50 and less than 2 characters");

            if (patchDoc != null)
            {
                var customer = _dealerRepository.Get(id);
                await _dealerRepository.JsonPatchWithModelState(customer, patchDoc, ModelState);


                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
