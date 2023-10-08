using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Controllers.MaterialControllers
{

    public partial class MaterialController
    {
        /// <summary>
        /// Making changes to one or more material fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "Texture",
        ///        "value": "Mate"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the Texture field of the selected dealer by id to "Mate"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        ///     Fields materail:
        ///     
        ///     "Id": can't be changed, integer
        ///     "Texture": "string", required, min 3 character
        ///     "Color": "string", min 2 character
        ///     "Size": float  more 1 and less 6
        ///     "Price": float, 0
        /// </remarks>
        /// <param name="id">Material ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Material changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no material for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("PatchJson")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<Material> patchDoc)
        {
            if (patchDoc != null)
            {
                var material = _materialRepository.Get(id);
                await _materialRepository.JsonPatchWithModelState(material, patchDoc, ModelState);
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return new ObjectResult(material);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
