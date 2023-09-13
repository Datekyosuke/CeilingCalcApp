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


        public Dealer Get(int id)
        {
            var customer = _context.Dealers.SingleOrDefault(p => p.Id == id);
            return customer;
        }
        public int Count()
        {
            return _context.Dealers.Count();
        }

        public List<Dealer> GetAll()
        {
            var pagedData = _context.Dealers.ToList();
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

        public List<Dealer> GetAll(PaginationFilter validFilter, string property, Sort sort, NumericRanges ranges)
        {
            if (sort == Sort.Asc)
            {
                var sortDealers = _context.Dealers
                            .Select(x => x)
                            .OrderBy(x => EF.Property<object>(x, property));
                var sortEntities = from Dealer entity in sortDealers
                                   where entity.Debts >= ranges.Min && entity.Debts <= ranges.Max
                                   select entity;
                return sortEntities
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToList();
            }
            else
            {
                var sortDealers = _context.Dealers
                           .Select(x => x)
                           .OrderByDescending(x => EF.Property<object>(x, property));
                var sortEntities = from Dealer entity in sortDealers
                                   where entity.Debts >= ranges.Min && entity.Debts <= ranges.Max
                                   select entity;
                return sortEntities
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .ToList();
            }
        
        }
    }
}
