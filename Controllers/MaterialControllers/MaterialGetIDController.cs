using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.MaterialControllers
{

    public partial class MaterialController 
    {
        /// <summary>
        /// Returns material by Id
        /// </summary>
        /// <param name="id">Material ID</param>
        /// <returns>Material</returns>
        /// <response code="200">Material retrieved</response>
        /// <response code="404">Material not found</response>
        /// <response code="500">Oops! Can't lookup your Material right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Dealer), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var material = _materialRepository.Get(id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(new Response<Material>(material));
        }
    }
}
