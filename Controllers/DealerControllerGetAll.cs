using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers
{
    /// <summary>
    /// Controller for working with dealers
    /// </summary>
    [Route("/api/[controller]")]
    public class DealerControllerGetAll : Controller
    {
       /* IRepository repo;
        public DealerControllerGetAll(IRepository r)
        {
            repo = r;
        }*/
        DealerContext db = new DealerContext();
        /// <summary>
        /// Returns a list of all dealers
        /// </summary>
        /// <returns>list dealers</returns>
        /// <response code="200">Dealers retrieved</response>
        [HttpGet()]
        public IEnumerable<Dealer> Get() => db.Dealers;

    }
}
