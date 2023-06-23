using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/DealerController")]
    public class DealerGetController : Controller
    {
  
        private IDealerRepository _dealerRepository;

        public DealerGetController(IDealerRepository dealerRepository)
        {
            _dealerRepository = dealerRepository;
        }
        /// <summary>
        /// Returns a list of all dealers
        /// </summary>
        /// <returns>list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        [HttpGet()]
        public IEnumerable<Dealer> GetAll()
        {
            return _dealerRepository.GetAll();
        }

    }
}
