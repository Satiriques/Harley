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
        public async Task<User> GetUserAsync(IUser user)
        {
            var userDb = await InnerGetUserAsync(user);

            if (userDb != null)
                return userDb;

            userDb = new User(user.Id);
            await InnerAddUserAsync(userDb);
            return userDb;
        }
    }
}
