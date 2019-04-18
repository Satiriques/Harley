using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using System.Linq;
using Discord;
using Discord.WebSocket;

namespace SatiriquesBot.Modules.Moderation
{
    public class ModerationModule : ModuleBase<SocketCommandContext>
    {
        [Command("cleanup")]
        public async Task CleanupAsync()
        {
            var chan = Context.Channel as SocketTextChannel;
            var messages = await chan.GetMessagesAsync(1000).FlattenAsync();
            var messagesToRemove = messages.Where(x => x.Content.StartsWith(".") || x.Author.IsBot);
            await chan.DeleteMessagesAsync(messagesToRemove);
        }
    }
}
