using Microsoft.EntityFrameworkCore;

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
