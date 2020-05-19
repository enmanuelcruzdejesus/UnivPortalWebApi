using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortalUnivWebApi.Services
{
    public class DatabaseContext: DbContext
    {
        private string _dbPath;
        public DatabaseContext() { }

        public DatabaseContext(string dbPath)
        {
            this._dbPath = dbPath;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_dbPath);
            base.OnConfiguring(optionsBuilder);
        }
    }
}
