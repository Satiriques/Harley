using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SatiriquesBot.Modules.Utils
{
    public class UtilsModule : ModuleBase<SocketCommandContext>
    {
        [Command("gethost")]
        public async Task GetHostAsync([Remainder]string url)
        {
            var uri = new Uri(url);
            await ReplyAsync(uri.Host);
        }
    }
}
