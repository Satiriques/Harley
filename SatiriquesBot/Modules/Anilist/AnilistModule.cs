using System.Threading.Tasks;
using Discord.Commands;
using Miki.Anilist;
using System.Linq;
using SatiriquesBot.Modules.Anilist.Helper;
using Discord;
using System;

namespace SatiriquesBot.Modules.Anilist
{
    public class AnilistModule : ModuleBase<SocketCommandContext>
    {
        [Command("manga")]
        public async Task MangeAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetMediaAsync(query, MediaFormat.MANGA);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }

        [Command("char")]
        public async Task CharacterAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetCharacterAsync(query);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }

        [Command("vizioz")]
        public async Task UserAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetCharacterAsync(query);
            string nom = ch.FirstName + ch.LastName;

            var embed = new EmbedBuilder()
            {
                Title = nom.PadLeft(5, ','),
                Url = ch.SiteUrl,
                Description = ch.Description,
                ThumbnailUrl = ch.LargeImageUrl,
                Color = Discord.Color.Red,
                Footer = new EmbedFooterBuilder()
                {
                    IconUrl = ch.LargeImageUrl,
                    Text = ch.Id.ToString()
                }
            }.Build();

            await ReplyAsync(embed: embed);
        }
    }
}
