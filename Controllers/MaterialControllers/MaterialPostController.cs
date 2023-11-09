using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.MaterialControllers
{
    public partial class MaterialController
    {
        /// <summary>
        /// Create new material
        /// </summary>
        /// <param name="material">All fields of the material, except id. ID is generated automatically, leave 0.</param>
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
            ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

            if (validationResult.IsValid)
            {
                await _materialRepository.Post(material);
                return Ok("Material created!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
