using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using MtgApiManager.Lib.Service;
using System.Linq;
using MoreLinq;
using Discord.Addons.Interactive;

namespace SatiriquesBot.Modules.Magic
{
    [Name("Magic the Gathering")]
    public class MagicModule : InteractiveBase<SocketCommandContext>
    {
        private readonly CardService _cardService;

        public MagicModule(CardService cardService)
        {
            _cardService = cardService;
        }

        [Command("mtg", RunMode = RunMode.Async)]
        public async Task CardAsync([Remainder]string name)
        {
            var result = await _cardService.Where(x => x.Name, name).AllAsync();
            var cards = result.Value.DistinctBy(x => x.Name).ToArray();
            var pages = cards.Select((x, i) => MagicHelper.BuildPage(x,i,cards.Length));
            if (result.IsSuccess && result.Value.Count > 0)
                await PagedReplyAsync(new PaginatedMessage() { Pages = pages }, new ReactionList() { Backward = true, First = true, Last = true, Trash = true });

        }
    }
}
