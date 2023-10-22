using Microsoft.AspNetCore.Mvc;


namespace WebApiDB.Controllers.DealerControllers
{


    public partial class DealerController 
    {
    
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
            var dealer = _dealerRepository.GetAsync(id).Result;

            if (dealer != null)
            {
               await _dealerRepository.Delete(dealer);
            }
            else return NotFound();
            return Ok("Dealer deleted!");
        }
    }
}
