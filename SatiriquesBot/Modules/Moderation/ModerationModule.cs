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
            var messagesToRemove = messages.Where(x => x.Content.StartsWith(";") || x.Author == Context.Guild.CurrentUser);
            await chan.DeleteMessagesAsync(messagesToRemove);
        }

        [Command("say")]
        [RequireOwner]
        public async Task SayAsync([Remainder]string text)
        {
            await ReplyAsync(Format.Code(text, ""));
        }
        
    }
}
