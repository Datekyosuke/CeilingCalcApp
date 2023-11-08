using Microsoft.AspNetCore.Mvc;
using WebApiDB.Helpers;
using WebApiDB.Models;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Making changes to one order record of a specific ID.
        /// </summary>
        /// <remarks>
        ///     Implementation of the Patch method through the Put method. In order not to change any field of the record, leave it as it is in Example Value
        ///     
        ///Fields order:
        ///
        ///  
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="order"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>

        [HttpPatch]
        public async Task<ActionResult> Patch(int id, [FromBody] OrderDTO dtoOrder)
        {
            var oldOrder = _orderRepository.GetAsync(id).Result;

            if (oldOrder == null)
                return NotFound();

            // TODO : add validation order


            /*var validation = ValidationDealer.DealerValidation(order);
            if (!validation.Item1)
            {
                return BadRequest(validation.Item2);
            }
            */
            dtoOrder.Id = id;
            if (dtoOrder.DealerId == 0)
                dtoOrder.DealerId = oldOrder.Dealer.Id;
            if (dtoOrder.DateOrder == default(DateTime))
                dtoOrder.DateOrder = oldOrder.DateOrder;
            if (dtoOrder.OperatorId == 0)
                dtoOrder.OperatorId = oldOrder.OperatorId;
            if (dtoOrder.Sum == 0)
                dtoOrder.Sum = oldOrder.Sum;
            if (dtoOrder.Status == "string")
                dtoOrder.Status = oldOrder.Status;
            
            var order = _mapper.Map<Order>(dtoOrder);


            await _orderRepository.Patch(oldOrder, order);

            return Ok("Order changed!");
        }
    }
}
