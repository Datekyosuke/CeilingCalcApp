using FuzzySharp;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Context;
using WebApiDB.Controllers.DealerControllers;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Servics;

namespace WebApiDB.Repository
{
    public class DealerReposytory : IDealerRepository
    {

        private readonly AplicationContext _context;
        private readonly IUriService _uriService;

        public DealerReposytory(AplicationContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;
        }


        public async Task Delete(Dealer dealer)
        {
            _context.Dealers.Remove(dealer);
            await _context.SaveChangesAsync();
        }


        public async Task<Dealer> GetAsync(int id)
        {
            var dealer = await _context.Dealers.FirstOrDefaultAsync(p => p.Id == id);
            return dealer;
        }

        public async Task JsonPatchWithModelState(Dealer dealer, JsonPatchDocument<Dealer> patchDoc, Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            patchDoc.ApplyTo(dealer, modelState);
            await _context.SaveChangesAsync();
        }

        public async Task Post(Dealer dealer)
        {
            _context.Add(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task Put(Dealer oldDealer, Dealer dealer)
        {
            _context.Entry(oldDealer).CurrentValues.SetValues(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task Patch(Dealer oldDealer, Dealer dealer)
        {
            _context.Entry(oldDealer).CurrentValues.SetValues(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<Dealer>>> GetAllAsync(PaginationFilter validFilter, string propertyCamelCase, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var firstChar = propertyCamelCase[0].ToString().ToUpper();
            var property = firstChar + propertyCamelCase.Substring(1);
            var sortDealers =
                        sort == "asc" ?
                        _context.Dealers
                        .Select(x => x)
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "desc" ?
                        _context.Dealers
                       .Select(x => x)
                       .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _context.Dealers
                        .Select(x => x);



            if (searchString is not null)
            {
                string[] propertySearch = { "LastName", "FirstName", "City", "Telephone" };
                var matches = SearchHelper.Search(sortDealers.ToList(), searchString, propertySearch);
                totalRecords = matches.Distinct().Count();


                var sortedSearchEntities = matches
                        .Where(x => x.Debts >= ranges.Min && x.Debts <= ranges.Max)
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return PaginationHelper.CreatePagedReponse<Dealer>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
            }
            totalRecords = sortDealers.Count();
            var sortedEntities = sortDealers
                       .Where(x => x.Debts >= ranges.Min && x.Debts <= ranges.Max)
                       .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                       .Take(validFilter.PageSize)
                       .ToList();
            return PaginationHelper.CreatePagedReponse<Dealer>(sortedEntities, validFilter, totalRecords, _uriService, route);


        }
    }
}
