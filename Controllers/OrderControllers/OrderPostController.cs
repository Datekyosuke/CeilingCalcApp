using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Create new order
        /// </summary>
        /// <param name="dealer">All fields of the order, except id. ID is generated automatically, leave 0.</param>
        /// <returns>New order</returns>
        /// <response code="200">Order created</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="500">Something went wrong.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] OrderDTO dtoOrder)
        {

            var order = _mapper.Map<Order>(dtoOrder);
            ValidationResult validationResult = await _validatorOrder.ValidateAsync(order);

            if (validationResult.IsValid)
            {
                await _orderRepository.Post(order);
                return Ok("Oder created!");
            }

            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }

    }
}
