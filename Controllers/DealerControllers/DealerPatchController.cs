using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerPatchController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerPatchController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }
        /// <summary>
        /// Making changes to one dealer record of a specific ID.
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

        [HttpPatch]
        public async Task<ActionResult> Put(int id, [FromBody] Dealer dealer)
        {
            var oldClient = _dealerRepository.Get(id);

            if (oldClient == null)
                return NotFound();

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

            _dealerRepository.Patch(oldClient, dealer);

            return Ok("Dealer changed!");
        }
    }
}