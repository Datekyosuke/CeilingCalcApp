﻿using Microsoft.EntityFrameworkCore;
using WebApiDB.Models;

namespace WebApiDB.Data
{
   
    public class DealerContext :DbContext
    {
        public DealerContext(DbContextOptions<DealerContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Dealer> Dealers { get; set; }

 
    }

    
}
