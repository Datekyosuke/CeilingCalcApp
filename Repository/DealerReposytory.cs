using Castle.Core.Resource;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web.Mvc;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;
using System;
using WebApiDB.Pagination;
using System.Linq.Expressions;
using WebApiDB.Helpers;

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
        public List<Dealer> GetAll(PaginationFilter validFilter)
        {
            var pagedData = _context.Dealers
                        .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                        .Take(validFilter.PageSize)
                        .ToList();
            return pagedData;
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

        public List<Dealer> GetAllSort(PaginationFilter validFilter, string property, Sort sort)
        {
            if (sort == Sort.Asc)
            {
                return  _context.Dealers
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .OrderBy(x => EF.Property<object>(x, property))
                            .ToList();
            }
            else
            {
                 return  _context.Dealers
                            .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                            .Take(validFilter.PageSize)
                            .OrderByDescending(x => EF.Property<object>(x, property))
                            .ToList();
            }
        
        }
    }
}
