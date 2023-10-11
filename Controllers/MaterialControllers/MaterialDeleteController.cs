using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;

namespace WebApiDB.Controllers.MaterialControllers
{

    public partial class MaterialController
    {
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
            var material = _materialRepository.GetAsync(id).Result;

            if (material != null)
            {
                await _materialRepository.Delete(material);
            }
            else return NotFound();
            return Ok("Material deleted!");
        }
    }
}
