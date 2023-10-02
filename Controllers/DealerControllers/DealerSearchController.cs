﻿using FuzzySharp;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using static WebApiDB.Helpers.LevenshteinDistance;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/DealerController")]
    [ApiController]
    public class DealerSearchController : ControllerBase
    {
        private IDealerRepository _dealerRepository;

        public DealerSearchController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }
        /// <summary>
        /// Implements fuzzy search dealer by LastName, FirstName, City
        /// </summary>
        /// <param name="searchString"> Строка для поиска</param>
        /// <returns>Dealer</returns>
        /// <response code="200">Dealer retrieved</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't lookup your Dealer right now</response>
        [HttpGet("Search")]
        [ProducesResponseType(typeof(Dealer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get([FromQuery] string searchString)
        {
            var dealers = _dealerRepository.GetAllAsync().Result;
            var matches = new List<Dealer>();
            foreach(var str in searchString.Split(' '))
            foreach (var dealer in dealers)
            {
                if (Fuzz.PartialRatio(dealer.LastName.ToLower(), str.ToLower()) >= 70)
                { matches.Add(dealer); continue; }
                if (Fuzz.PartialRatio(dealer.FirstName.ToLower(), str.ToLower()) >= 70)
                { matches.Add(dealer); continue; }
                if (Fuzz.PartialRatio(dealer.City.ToLower(), str.ToLower()) >= 70)
                { matches.Add(dealer); continue; }

            }
            return Ok(matches.Distinct());
        }
    }
}
