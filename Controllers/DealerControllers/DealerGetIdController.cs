﻿using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.DealerControllers
{


    public partial class DealerController 
    {
      
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
            var dealer = _dealerRepository.GetAsync(id).Result;

            if (dealer == null)
            {
                return NotFound();
            }

            return Ok(new Response<Dealer>(dealer));
        }
    }
}
