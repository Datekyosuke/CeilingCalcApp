using FuzzySharp;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Controllers.DealerControllers;
using WebApiDB.Data;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;
using WebApiDB.Servics;

namespace WebApiDB.Repository
{
    public class DealerReposytory : IDealerRepository
    {

        private readonly DealerContext _context;
        private readonly IUriService _uriService;

        public DealerReposytory(DealerContext context, IUriService uriService)
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
            var customer = await _context.Dealers.FirstOrDefaultAsync(p => p.Id == id);
            return customer;
        }
        public int Count()
        {
            return _context.Dealers.Count();
        }

        //public async Task<List<Dealer>> GetAllAsync()
        //{
        //    var pagedData = await _context.Dealers.AllAsync();
        //    return pagedData;
        //}


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

        public async Task Put(Dealer oldClient, Dealer dealer)
        {
            _context.Entry(oldClient).CurrentValues.SetValues(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task Patch(Dealer oldClient, Dealer dealer)
        {
            _context.Entry(oldClient).CurrentValues.SetValues(dealer);
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResponse<List<Dealer>>> GetAllAsync(PaginationFilter validFilter, string property, string sort, NumericRanges ranges, string searchString, string? route)
        {
            var totalRecords = 0;
            var sortDealers = 
                        sort == "Asc" ?
                        _context.Dealers
                        .Select(x => x)
                        .OrderBy(x => EF.Property<object>(x, property)) :

                        sort == "Desc" ?
                        _context.Dealers
                       .Select(x => x)
                       .OrderByDescending(x => EF.Property<object>(x, property)) :

                        _context.Dealers
                        .Select(x => x);

            var matches = new List<Dealer>();
            if (searchString is not null)
            {
                foreach (var str in searchString.Split(' '))
                    foreach (var dealer in sortDealers)
                    {
                        if (Fuzz.PartialRatio(dealer.LastName.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(dealer); continue; }
                        if (Fuzz.PartialRatio(dealer.FirstName.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(dealer); continue; }
                        if (Fuzz.PartialRatio(dealer.City.ToLower(), str.ToLower()) >= 70)
                        { matches.Add(dealer); continue; }

                    }
                totalRecords = matches.Distinct().Count();
                var sortedSearchEntities = matches
                        .Where(x => x.Debts >= ranges.Min && x.Debts <= ranges.Max)
                        .Distinct()
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
                return  PaginationHelper.CreatePagedReponse<Dealer>(sortedSearchEntities, validFilter, totalRecords, _uriService, route);
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
