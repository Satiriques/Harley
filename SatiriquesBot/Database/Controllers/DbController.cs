using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace SatiriquesBot.Database.Controllers
{
    public abstract class DbController<T> where T : DbContext
    {
        internal readonly T _db;

        public DbController(T db)
        {
            _db = db;
        }
    }
}
