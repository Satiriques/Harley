using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Discord;
using Discord.Addons.Interactive;

namespace SatiriquesBot.Database.Controllers
{
    public partial class UserController : DbController<UserContext>
    {
        public async Task<User> GetUserAsync(IUser user)
        {
            var userDb = await InnerGetUserAsync(user);

            if (userDb != null)
                return userDb;

            userDb = new User(user.Id) { Pages = new UserPage[] { } };
            await InnerAddUserAsync(userDb);
            return userDb;
        }

        public async Task AddPageAsync(IUser user, TemplateType type, string url)
        {
            var dbUser = await GetUserAsync(user);

            if (dbUser.Pages.Any(x => x.TemplateType == type))
                return;

            await InnerAddPageAsync(new UserPage() { TemplateType = type, DiscordId = user.Id, Url = url });
        }
    }
}
