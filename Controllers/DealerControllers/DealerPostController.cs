﻿using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    public partial class DealerController 
    {
        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] DealerDTOGet dealerDtoGet)
        {
            var dealer = _mapper.Map<Dealer>(dealerDtoGet);
            var validationResult = _validatorDealer.Validate(dealer);
            if (validationResult.IsValid)
            {

                await _dealerRepository.Post(dealer);
                return Ok("Dealer created!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

    }
}
