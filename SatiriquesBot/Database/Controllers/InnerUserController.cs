using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Entities;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Discord;

namespace SatiriquesBot.Database.Controllers
{
    public partial class UserController : DbController<UserContext>
    {
        public UserController(UserContext db) : base(db) { }

        private async Task<User> InnerGetUserAsync(IUser user)
        { 
            var result = await _db.Users.AsQueryable().SingleOrDefaultAsync(x => x.DiscordId == user.Id);
            if (result == null)
                return result;

            result.Pages = (await InnerGetPagesAsync(user)) ?? new UserPage[] {};
            return result;
        }
        private async Task<UserPage[]> InnerGetPagesAsync(IUser user)
            => await _db.Pages.AsQueryable().Where(x => x.DiscordId == user.Id).ToArrayAsync();

        private async Task InnerAddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

        private async Task InnerAddPageAsync(UserPage page)
        {
            await _db.Pages.AddAsync(page);
            await _db.SaveChangesAsync();
        }
    }
}
