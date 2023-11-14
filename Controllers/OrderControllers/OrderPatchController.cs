using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data.DTO_Order;
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
            var order = _mapper.Map<Order>(dtoOrder);

            ValidationResult validationResult = await _validatorOrder.ValidateAsync(order);

            if (validationResult.IsValid)
            {
                order.Id = id;
                if (order.DealerId == 0)
                    order.DealerId = oldOrder.Dealer.Id;
                if (order.DateOrder == default(DateTime))
                    order.DateOrder = oldOrder.DateOrder;
                if (order.OperatorId == 0)
                    order.OperatorId = oldOrder.OperatorId;
                if (order.Sum == 0)
                    order.Sum = oldOrder.Sum;
                if (order.Status == "string")
                    order.Status = oldOrder.Status;

                await _orderRepository.Patch(oldOrder, order);
                return Ok("Order changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
