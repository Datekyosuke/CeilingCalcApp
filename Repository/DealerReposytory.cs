using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
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


        public IActionResult Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Dealer> GetAll() => _context.Dealers;
       

        public Task<IActionResult> JsonPatchWithModelState(int id, JsonPatchDocument<Dealer> patchDoc)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> Post(Dealer dealer)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Put(int id, Dealer dealer)
        {
            throw new NotImplementedException();
        }
    }
}
