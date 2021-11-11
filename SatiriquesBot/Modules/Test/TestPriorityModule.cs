using Discord.Commands;
using System.Threading.Tasks;

namespace SatiriquesBot.Modules.Test
{
    public class TestPriorityModule : ModuleBase<SocketCommandContext>
    {
        [Priority(100)]
        [Command("testprio")]
        public async Task TestPriorityAsync(int id)
        {
            await ReplyAsync("Ceci est un chiffre");
        }

        [Priority(1)]
        [Command("testprio")]
        public async Task TestPriorityAsync(string text)
        {
            await ReplyAsync("Ceci est du text");
        }
    }
}
