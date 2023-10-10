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

        public async Task<IEnumerable<Dealer>> Get(string propertyCamelCase, string sort)
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

            //    var matches = new List<Dealer>();
            //    foreach (var dealer in sortDealers)
            //    {
            //        var flag = searchString.Split(' ').Count();
            //        foreach (var str in searchString.Split(' '))
            //        {
            //            if (Fuzz.PartialRatio(dealer.LastName.ToLower(), str.ToLower()) >= 70)
            //            { flag--; continue; }
            //            if (Fuzz.PartialRatio(dealer.FirstName.ToLower(), str.ToLower()) >= 70)
            //            { flag--; continue; }
            //            if (Fuzz.PartialRatio(dealer.City.ToLower(), str.ToLower()) >= 70)
            //            { flag--; continue; }

            //        }
            //        if(flag == 0)
            //            matches.Add(dealer);
            //    }
 
            

           
            return sortDealers.ToList();


        }
    }
}
