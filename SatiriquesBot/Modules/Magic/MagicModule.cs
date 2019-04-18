using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using MtgApiManager.Lib.Service;

namespace SatiriquesBot.Modules.Magic
{
    public class MagicModule : ModuleBase<SocketCommandContext>
    {
        private readonly CardService _cardService;

        public MagicModule(CardService cardService)
        {
            _cardService = cardService;
        }

        [Command("card")]
        public async Task CardAsync([Remainder]string name)
        {
            var result = await _cardService.Where(x => x.Name, name).AllAsync();
            if (result.IsSuccess && result.Value.Count > 0)
                await ReplyAsync(embed: MagicHelper.BuildEmbed(result.Value.ToArray()[0]));

        }
    }
}
