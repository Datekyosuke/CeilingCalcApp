using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Controllers.OrderControllers

{
    [Route("/api/OrderController")]
    [ApiController]
    public partial class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private IValidator<Order> _validatorOrder;


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
        ///     "dillerFullName": "string"
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
            var pagedReponse = _orderRepository.GetAllAsync(validFilter, expression, sort, ranges, trimSearchString, route).Result;
            return Ok(pagedReponse);
        }
    }
}
