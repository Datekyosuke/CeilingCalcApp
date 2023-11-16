using AutoMapper;
using CeilingCalc.Interfaces;
using CeilingCalc.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Context;
using WebApiDB.Data.DTO_Order;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace CeilingCalc.Repository
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly AplicationContext _context;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;


        public OrderDetailRepository(AplicationContext context, IUriService uriService, IMapper mapper)
        {
            _context = context;
            _uriService = uriService;
            _mapper = mapper;
        }

        public async Task Delete(OrderDetail orderDetail)
        {
            _context.OrderDetails.Remove(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task<OrderDetail> GetAsync(int id)
        {
            var orderDetail = await _context.OrderDetails.Include(o => o.Material).Include(d => d.Order).Where(x => x.Id == id).FirstOrDefaultAsync();
            return orderDetail;
        }
        public async Task<PagedResponse<List<OrderDetail>>> GetAllAsync(PaginationFilter validFilter, string propertyCamelCase, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var firstChar = propertyCamelCase[0].ToString().ToUpper();
            var property = firstChar + propertyCamelCase.Substring(1);

            var sortDealers =
                         sort == "asc" ?
                         _context.OrderDetails
                         .Include(x => x.Material)
                         .Include(o => o.Order)
                         .Select(x => x)
                        .AsQueryable()
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "desc" ?
                        _context.OrderDetails
                         .Include(x => x.Material)
                         .Include(o => o.Order)
                         .Select(x => x)
                         .AsQueryable()
                         .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _context.OrderDetails
                         .Include(x => x.Material)
                         .Include(o => o.Order)
                         .Select(x => x)
                        .AsQueryable();



            if (searchString is not null)
            {
                string[] propertySearch = { "Sum", "Price"};
                var matches = SearchHelper.Search(sortDealers.ToList(), searchString, propertySearch);
                totalRecords = matches.Distinct().Count();


                var sortedSearchEntities = matches
                        .Where(x => x.Sum >= ranges.Min && x.Sum <= ranges.Max)
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return PaginationHelper.CreatePagedReponse<OrderDetail>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            }
            totalRecords = sortDealers.Count();
            var sortedEntities = sortDealers
                       .Where(x => x.Sum >= ranges.Min && x.Sum <= ranges.Max)
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToList();
            return PaginationHelper.CreatePagedReponse<OrderDetail>(sortedEntities, validFilter, totalRecords, _uriService, route);


        }

        public async Task JsonPatchWithModelState()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Patch(OrderDetail oldOrderDetail, OrderDetail orderDetail)
        {
            _context.Entry(oldOrderDetail).CurrentValues.SetValues(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task Post(OrderDetail orderDetail)
        {
            _context.Add(orderDetail);
            await _context.SaveChangesAsync();
        }

        public async Task Put(OrderDetail oldOrderDetail, OrderDetail orderDetail)
        {
            _context.Entry(oldOrderDetail).CurrentValues.SetValues(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
