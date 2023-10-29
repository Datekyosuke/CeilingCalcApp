﻿using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using Microsoft.AspNetCore.Mvc;
using WebApiDB.Context;
using AutoMapper;
using WebApiDB.Data.DTO_Order;

namespace WebApiDB.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AplicationContext _context;
        private readonly IUriService _uriService;


        public OrderRepository(AplicationContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }

        public async Task Delete(Order order)
        {
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }

        public async Task<Order> GetAsync(int id)
        { 
            var order = await _context.Orders.Include(o => o.Dealer).Where(x => x.Id == id).FirstOrDefaultAsync();
            return order;
        }
        public async Task<PagedResponse<List<OrderG>>> GetAllAsync(PaginationFilter validFilter, string propertyCamelCase, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var firstChar = propertyCamelCase[0].ToString().ToUpper();
            var property = firstChar + propertyCamelCase.Substring(1);

            var sortDealers =
                        _context.Orders
                        .Include(x => x.Dealer)
                         .Select(x => new OrderG
                         {
                             Id = x.Id,
                             DateOrder = x.DateOrder,
                             OperatorId = x.OperatorId,
                             Sum = x.Sum,
                             Status = x.Status,
                             DealerName = x.Dealer.LastName
                         }).
                         AsQueryable()
                        .OrderBy(x => EF.Property<object>(x, property));

            /*var sortDealers =
                        sort == "asc" ?
                        _context.Orders
                        .Include(x => x.Dealer)
                         .Select(x => new
                         {
                             x.Id,
                             x.DateOrder,
                             x.OperatorId,
                             x.Sum,
                             x.Status,
                             x.Dealer.LastName
                         }).
                         AsEnumerable()
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "desc" ?
                        _context.Orders
                       .Select(x => x)
                       .Include(x => x.Dealer)
                       .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _context.Orders
                        .Include(x => x.Dealer)
                        .Select(x => x);*/



            if (searchString is not null)
            {
                string[] propertySearch = { "Status" };
                var matches = SearchHelper.Search(sortDealers.ToList(), searchString, propertySearch);
                totalRecords = matches.Distinct().Count();


                var sortedSearchEntities = matches
                        .Where(x => x.Sum >= ranges.Min && x.Sum <= ranges.Max)
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return PaginationHelper.CreatePagedReponse<OrderG>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            }
            totalRecords = sortDealers.Count();
            var sortedEntities = sortDealers
                       .Where(x => x.Sum >= ranges.Min && x.Sum <= ranges.Max)
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToList();
            return PaginationHelper.CreatePagedReponse<OrderG>(sortedEntities, validFilter, totalRecords, _uriService, route);


        }

        public async Task JsonPatchWithModelState(Order order, JsonPatchDocument<Order> patchDoc, ModelStateDictionary modelState)
        {
            patchDoc.ApplyTo(order, modelState);
            await _context.SaveChangesAsync();
        }

        public async Task Patch(Order oldOrder, Order order)
        {
            _context.Entry(oldOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
        }

        public async Task Post(Order order)
        {
            order.Dealer = _context.Dealers.FirstOrDefault(x => x.Id == order.Id);
             _context.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task Put(Order oldOrder, Order order)
        {
            _context.Entry(oldOrder).CurrentValues.SetValues(order);
            await _context.SaveChangesAsync();
        }
    }
}
