using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.MaterialControllers
{
    [Route("api/MaterialController")]
    [ApiController]
    public class MaterialPostController : Controller
    {
        private IMaterialRepository _materialRepository;

        public MaterialPostController(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        /// <summary>
        /// Create new material
        /// </summary>
        /// <param name="dealer">All fields of the material, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New material</returns>
        /// <response code="200">Material created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Material material)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (material.Texture.Length < 3 || material.Texture.Length > 50)
                return BadRequest("Texture cannot be more than 50 and less than 3 characters");
            if (material.Color.Length > 50 || material.Color.Length < 2)
                return BadRequest("Color cannot be more than 50 and less than 2 characters");
            await _materialRepository.Post(material);
            return Ok("Material created!");
        }
    }
}
