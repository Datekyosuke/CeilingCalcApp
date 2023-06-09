﻿using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerPostController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerPostController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }

        /// <summary>
        /// Create new dealer
        /// </summary>
        /// <param name="dealer">All fields of the dealer, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New dealer</returns>
        /// <response code="200">Dealer created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Dealer dealer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
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
            await _dealerRepository.Post(dealer);
            return Ok("Dealer created!");
        }

    }
}
