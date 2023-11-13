﻿using CeilingCalc.Data.DTO_Material;
using FluentValidation.Results;
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
        public async Task<ActionResult> Patch(int id, [FromBody] MaterialDTO materialDTO)
        {
            var material = _mapper.Map<Material>(materialDTO);
            var oldMaterial = _materialRepository.GetAsync(id).Result;

            if (oldMaterial == null)
                return NotFound();
            ValidationResult validationResult = await _validatorMaterial.ValidateAsync(material);

            if (validationResult.IsValid)
            {

                if (material.Texture == "string")
                    material.Texture = oldMaterial.Texture;
                if (material.Color == "string")
                    material.Color = oldMaterial.Color;
                if (material.Size == 0)
                    material.Size = oldMaterial.Size;
                if (material.Price == 0)
                    material.Price = oldMaterial.Price;

                await _materialRepository.Patch(oldMaterial, material);

                return Ok("Material changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
