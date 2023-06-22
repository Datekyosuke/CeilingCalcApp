using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDB.Data;
using WebApiDB.Interfaces;
using WebApiDB.Models;

namespace WebApiDB.Repository
{
    public class DealerReposytory : IDealerRepository
    {

        private readonly DealerContext _context;

        public DealerReposytory(DealerContext context)
        {
            _context = context;
        }
        public Task<IActionResult> Delete()
        {
            throw new NotImplementedException();
        }


        public Dealer Get(int id)
        {
            return _context.Dealers.SingleOrDefault(p => p.Id == id);
                        
        }

        public IEnumerable<Dealer> GetAll() => _context.Dealers;
       

        public Task<IActionResult> JsonPatchWithModelState(int id, JsonPatchDocument<Dealer> patchDoc)
        {
            throw new NotImplementedException();
        }

        public async Task Post(Dealer dealer)
        {
            _context.Add(dealer);
            await _context.SaveChangesAsync();
        }

        public Task<ActionResult> Put(int id, Dealer dealer)
        {
            throw new NotImplementedException();
        }
    }
}
