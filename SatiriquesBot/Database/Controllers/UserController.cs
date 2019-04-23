using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;
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
            => await _db.Users.SingleOrDefaultAsync(x => x.DiscordId == user.Id);
        private async Task<Page> InnerGetPagesAsync(IUser user)
            => await _db.Pages.SingleOrDefaultAsync(x => x.DiscordId == user.Id);

        private async Task InnerAddUserAsync(User user)
        {
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }

    }
}
