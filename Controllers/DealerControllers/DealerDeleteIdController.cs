using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data;
using WebApiDB.Models;

namespace WebApiDB.Controllers.DealerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealerDeleteIdController : ControllerBase
    {
        private readonly DealerContext db;

        public DealerDeleteIdController(DealerContext _db)
        {
            db = _db;
        }
        /// <summary>
        /// Removes dealer by id
        /// </summary>
        /// <param name="id">Dealer ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Dealer removed</response>
        /// <response code="404">Dealer not found</response>
        /// <response code="500">Oops! Can't remove your Dealer right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dealer = db.Dealers.SingleOrDefault(p => p.Id == id);

            if (dealer != null)
            {
                db.Dealers.Remove(db.Dealers.SingleOrDefault(p => p.Id == id));
                await db.SaveChangesAsync();
            }
            else return NotFound();
            return Ok("Dealer deleted!");
        }
    }
}
