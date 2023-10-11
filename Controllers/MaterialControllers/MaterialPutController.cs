using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Repository;

namespace WebApiDB.Controllers.MaterialControllers
{
    public partial class MaterialController
    {
        /// <summary>
        /// Making changes to one material record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Fields dealer:
        /// 
        ///    "Id": can't be changed, integer
        ///    
        ///     "Texture": "string", required, minimum 3 character
        ///     
        ///     "Color": "string", minimum 2 character
        ///     
        ///     "Size": float, Size should be between 1 and 6, may be null
        ///     
        ///     "Price": float, 0
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="material"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no dealer for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] Material material)
        {
            var oldMaterial = _materialRepository.GetAsync(id).Result;
            if (oldMaterial == null)
                return NotFound();

            if (material.Texture.Length > 50 || material.Texture.Length < 2)
                return BadRequest("Texture cannot be more than 50 and less than 2 characters");
            if (material.Price > float.MaxValue || material.Price < float.MinValue)
                return BadRequest("Wrong debts! Too big (small) number");
            if (material.Size < 1 || material.Size > 6)
                return BadRequest("Size should be between 1 and 6");

            await _materialRepository.Put(oldMaterial, material);


            return Ok("Material changed!");
        }
    }
}
