using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Making changes to one or more dealer fields
        /// </summary>
        /// <remarks>
        ///  Request example:
        ///
        ///     [
        ///     {
        ///        "op": "add",
        ///        "path": "Status",
        ///        "value": "In work"
        ///     }
        ///     ]
        ///
        /// This example changes the value of the Status field of the selected order by id to "In work"
        /// 
        ///     See more: https://learn.microsoft.com/ru-ru/aspnet/core/web-api/jsonpatch?view=aspnetcore-7.0#path-syntax
        ///     
        /// Properties can take Order field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "dealerId": integer, required
        ///     "DateOrder": "date time", not null, in format DateTime(2015, 7, 20, 18, 30, 25); // год - месяц - день - час - минута - секунда
        ///     "OperatorId": integer, who added order  
        ///     "Sum": float, full sum of order
        ///     "Status": "string",  status of order. in work, in calculation etc    
        ///     
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="patchDoc"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>
        [HttpPatch("PatchJson")]
        public async Task<IActionResult> JsonPatchWithModelState(int id,
        [FromBody] JsonPatchDocument<Order> patchDoc)
        {
            //TODO: validation order!

            if (patchDoc.Operations[0].path.ToLower() == "id")
                return BadRequest("id cannot be changes");

            if (patchDoc.Operations[0].path.ToLower() == "dateorder")
            {
                if (!DateTime.TryParse(patchDoc.Operations[0].value.ToString(), out DateTime dateTime))
                    return BadRequest("Wrong date time! Must be a number");
                else if (dateTime > DateTime.Now)
                    return BadRequest("Wrong date! Not future");
            }

            if (patchDoc.Operations[0].path.ToLower() == "firstname" && patchDoc.Operations[0].value.ToString().Length > 50)
                return BadRequest("FirstName cannot be more than 50 characters");

            if (patchDoc.Operations[0].path.ToLower() == "sum")
            {
                {
                    if (patchDoc.Operations[0].value == "")
                        patchDoc.Operations[0].value = 0;
                    if (!float.TryParse(patchDoc.Operations[0].value.ToString(), out float sum))
                        return BadRequest("Wrong debts! Must be a number");
                    else if (sum < float.MinValue || sum > float.MaxValue)
                        return BadRequest("Wrong sum! Too big (small) number");
                }

            }
            if (patchDoc.Operations[0].path.ToLower() == "status" && (patchDoc.Operations[0].value.ToString().Length > 50))
                return BadRequest("Status cannot be more than 50 and less than 2 characters");

            if (patchDoc != null)
            {
                var customerDTO = _orderRepository.GetAsync(id).Result;
                var customer = _mapper.Map<Order>(customerDTO);
                await _orderRepository.JsonPatchWithModelState(customer, patchDoc, ModelState);
                return new ObjectResult(customer);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
