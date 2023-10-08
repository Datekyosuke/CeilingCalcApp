using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using WebApiDB.Data;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{

    public partial class DealerController 
    {
   
        /// <summary>
        /// Making changes to one dealer record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] Dealer dealer)
        {
            var oldClient = _dealerRepository.GetAsync(id).Result;

            if (oldClient == null)
                return NotFound();

            var validation = ValidationDealer.DealerValidation(dealer);
            if (!validation.Item1)
            {
                return BadRequest(validation.Item2);
            }
            if (dealer.FirstName == "string")
                dealer.FirstName = oldClient.FirstName;
            if (dealer.LastName == "string")
                dealer.LastName = oldClient.LastName;
            if (dealer.Telephone == 0)
                dealer.Telephone = oldClient.Telephone;
            if (dealer.Debts == 0)
                dealer.Debts = oldClient.Debts;
            if (dealer.City == "string")
                dealer.City = oldClient.City;


          

            await _dealerRepository.Patch(oldClient, dealer);

            return Ok("Dealer changed!");
        }
    }
}