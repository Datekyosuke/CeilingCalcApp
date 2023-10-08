using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.MaterialControllers
{
    public partial class MaterialController
    {
        /// <summary>
        /// Making changes to one material record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields dealer:
        ///
        ///     "Id": can't be changed, integer
        ///     "Texture": "string", required
        ///     "Color": "string", min 2 character
        ///     "Size": float, may be bull
        ///     "Price": float, may be bull
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="material"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no material for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] Material material)
        {
            var oldMaterial = _materialRepository.Get(id);

            if (oldMaterial == null)
                return NotFound();

            if (material.Texture == "string")
                material.Texture = oldMaterial.Texture;
            if (material.Color == "string")
                material.Color = oldMaterial.Color;
            if (material.Size == 0)
                material.Size = oldMaterial.Size;
            if (material.Price == 0)
                material.Price = oldMaterial.Price;
                       
            if (material.Texture.Length > 50 || material.Texture.Length < 2)
                return BadRequest("Texture cannot be more than 50 and less than 2 characters");
            if (material.Price > float.MaxValue || material.Price < float.MinValue)
                return BadRequest("Wrong debts! Too big (small) number");
            if (material.Size < 1 || material.Size > 6)
                return BadRequest("Size should be between 1 and 6");

            await _materialRepository.Patch(oldMaterial, material);

            return Ok("Material changed!");
        }
    }
}
