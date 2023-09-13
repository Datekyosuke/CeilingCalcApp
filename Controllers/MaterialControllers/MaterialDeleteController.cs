using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;

namespace WebApiDB.Controllers.MaterialControllers
{
    [Route("api/MaterialController")]
    [ApiController]
    public class MaterialDeleteController : Controller
    {
        private IMaterialRepository _materialRepository;

        public MaterialDeleteController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }
        /// <summary>
        /// Removes material by id
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Material removed</response>
        /// <response code="404">Material not found</response>
        /// <response code="500">Oops! Can't remove your Material right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dealer = _materialRepository.Get(id);

            if (dealer != null)
            {
                await _materialRepository.Delete(dealer);
            }
            else return NotFound();
            return Ok("Material deleted!");
        }
    }
}
