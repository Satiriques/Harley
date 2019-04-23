using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SatiriquesBot.Database.Controllers;

namespace SatiriquesBot.Modules.Social
{
    public class SocialModule : ModuleBase<SocketCommandContext>
    {
        private UserController _db;
        public SocialModule(UserController controller)
        {
            _db = controller;
        }


        [Command("profile")]
        public async Task SocialModuleAsync()
        {
            await ReplyAsync((await _db.GetUserAsync(Context.User)).ToString());
        }
    }
}
