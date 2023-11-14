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
            if (patchDoc.Operations[0].path.ToLower() == "id")
                return BadRequest("id cannot be changes");

            if (patchDoc.Operations[0].path.ToLower() == "texture" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("Texture cannot be more than 50 and less than 2 characters");

            if (patchDoc.Operations[0].path.ToLower() == "color" && (patchDoc.Operations[0].value.ToString().Length > 50 || patchDoc.Operations[0].value.ToString().Length < 2))
                return BadRequest("Color cannot be more than 50 and less than 2 characters");
            if (patchDoc.Operations[0].path.ToLower() == "size")
            {
                {
                    if (patchDoc.Operations[0].value == "")
                        patchDoc.Operations[0].value = 0;
                    if (!float.TryParse(patchDoc.Operations[0].value.ToString(), out float debts))
                        return BadRequest("Wrong size! Must be a number");
                    else if (debts < float.MinValue || debts > float.MaxValue)
                        return BadRequest("Wrong size! Too big (small) number");
                }

            }
            if (patchDoc.Operations[0].path.ToLower() == "price")
            {
                {
                    if (patchDoc.Operations[0].value == "")
                        patchDoc.Operations[0].value = 0;
                    if (!float.TryParse(patchDoc.Operations[0].value.ToString(), out float debts))
                        return BadRequest("Wrong price! Must be a number");
                    else if (debts < float.MinValue || debts > float.MaxValue)
                        return BadRequest("Wrong price! Too big (small) number");
                }

            }
            if (patchDoc != null)
            {
                var material = _materialRepository.GetAsync(id).Result;
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
