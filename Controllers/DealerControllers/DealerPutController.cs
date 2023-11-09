using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{

    public partial class DealerController 
    {

        /// <summary>
        /// Making changes to one dealer record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "firstName": "string", required
        ///     "lastName": "string", may be null
        ///     "telephone": 11 digit, integer
        ///     "debts": 0, integer
        ///     "city": "string",  required    
        ///     
        /// </remarks>
        /// <param name="id">Dealer ID</param>
        /// <param name="dealer"></param>
        /// <response code="200">Dealer changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] DTODealer dTOdealer)
        {
            var dealer = _mapper.Map<Dealer>(dTOdealer);
            var oldClient = _dealerRepository.GetAsync(id).Result;

            if (oldClient == null)
                return NotFound();

            var validationResult = _validatorDealer.Validate(dealer);

            if (validationResult.IsValid)
            {
                await _dealerRepository.Put(oldClient, dealer);
                return Ok("Dealer changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
