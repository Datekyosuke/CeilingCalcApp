using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Models;

namespace WebApiDB.Controllers.OrderControllers
{
    public partial class OrderController : Controller
    {
        /// <summary>
        /// Making changes to one oder record of a specific ID
        /// </summary>
        /// <remarks>
        /// 
        ///  Warning! Unfilled fields will be assigned a default value, as in the scheme
        /// 
        /// Properties can take Order field values:
        /// 
        ///     
        ///     
        /// </remarks>
        /// <param name="id">Order ID</param>
        /// <param name="order"></param>
        /// <response code="200">Order changed</response>
        /// <response code="400">Something went wrong. Possibly invalid request body.</response>
        /// <response code="404">There is no order for this id</response>
        /// <response code="500">Something went wrong. Possibly invalid request body.</response>


        [HttpPut]
        public async Task<ActionResult> Put(int id, [FromBody] OrderDTO dtoOrder)
        {
            var oldOrder = _orderRepository.GetAsync(id).Result;
            if (oldOrder == null)
                return NotFound();
            var order = _mapper.Map<Order>(dtoOrder);

            ValidationResult validationResult = await _validatorOrder.ValidateAsync(order);

            if (validationResult.IsValid)
            {
                await _orderRepository.Put(oldOrder, order);
                return Ok("Order changed!");
            }
            var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
            return BadRequest(errorMessages);
        }
    }
}
