using FuzzySharp;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Helpers;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using WebApiDB.Pagination;

namespace WebApiDB.Repository
{
    public class DealerReposytory : IDealerRepository
    {

        private readonly DealerContext _context;

        public DealerReposytory(DealerContext context)
        {
            _context = context;
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

        public async Task<List<Dealer>> GetAllAsync()
        {
            var pagedData = await _context.Dealers.ToListAsync();
            return pagedData;
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

        public async Task<List<Dealer>> GetAllAsync(PaginationFilter validFilter, string property, string sort, NumericRanges ranges, string searchString)
        {

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
                matches.Distinct().ToList();
            }
            else matches = await sortDealers.ToListAsync();

            var sortEntities = from Dealer entity in matches
                               where entity.Debts >= ranges.Min && entity.Debts <= ranges.Max
                                   select entity;

            return       sortEntities
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
            

        }
    }
}
