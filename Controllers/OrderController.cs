using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace CeilingCalc.Controllers
{
    /// <summary>
    /// Controller for working with order
    /// </summary>
    
    [Route("/api/OrderController")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<Order> _validatorOrder;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, IValidator<Order> validatorOrder)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _validatorOrder = validatorOrder;
        }

        /// <summary>
        /// Returns a paginated, sorted and ranged list of orders. 
        /// </summary>
        /// <remarks>
        /// Page number must be greater than or equal to 0 and PageSize greater than or equal to 1. 
        /// If PageNumber = 0, displays the entire list of oreders.
        /// 
        /// Properties can take Dealer field values:
        /// 
        ///     "id": can't be changed, integer
        ///     "dillerId": intege, requered, must be exist
        ///     "dateOrder": "date", regured, auto time
        ///     "OperatorId": id who added the order, integer
        ///     "Sum": full sum this order, float
        ///     "status": "string",  work/not work/on calculation etc    
        /// Sort 
        /// 
        ///     asc - ascending
        ///     desc - descending
        ///     else without sorting
        ///     
        ///  min  - search for sum from
        /// 
        ///  max - search for sum up
        /// </remarks>
        /// <returns>Page list orders</returns>
        /// <response code="200">Orders retrieved</response>
        ///  <response code="400">Wrong request body</response>
        [HttpGet()]
        public IActionResult GetAll([FromQuery] PaginationFilter filter, [FromQuery] Orderable orderable, [FromQuery] NumericRanges ranges, [FromQuery] string? searchString)
        {
            if (ranges.Max < ranges.Min) return BadRequest("Maximum must be greater than or equal to the minimum");
            var route = Request.Path.Value;
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var expression = orderable.Property;
            var sort = orderable.Sort;
            var trimSearchString = searchString?.Trim();
            var pagedReponse = _orderRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString).Result;
            return Ok(pagedReponse);
        }

        /// <summary>
        /// Returns order by Id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Order</returns>
        /// <response code="200">Order retrieved</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't lookup your Order right now</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Order), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult Get(int id)
        {
            var order = _mapper.Map<OrderG>(_orderRepository.GetAsync(id).Result);
            if (order == null)
            {
                return NotFound();
            }
            return Ok(new Response<OrderG>(order));
        }

        /// <summary>
        /// Removes order by id
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <returns>Void</returns>
        /// <response code="200">Order removed</response>
        /// <response code="404">Order not found</response>
        /// <response code="500">Oops! Can't remove your Order right now</response>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var order = _orderRepository.GetAsync(id).Result;

            if (order != null)
            {
                await _orderRepository.Delete(order);
            }
            else return NotFound();
            return Ok("Order deleted!");
        }

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
                if (order.DateOrder == default)
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

            if (patchDoc != null)
            {
                var order = _orderRepository.GetAsync(id).Result;
                patchDoc.ApplyTo(order);
                ValidationResult validationResult = await _validatorOrder.ValidateAsync(order);

                if (validationResult.IsValid)
                {
                    await _orderRepository.JsonPatchWithModelState();
                    return new ObjectResult(order);
                }

                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return BadRequest(errorMessages);


            }
            else
            {
                return BadRequest(ModelState);
            }
        }

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
