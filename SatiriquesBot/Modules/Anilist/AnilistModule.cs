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
        [Alias("m")]
        public async Task MangeAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetMediaAsync(query, MediaFormat.MANGA);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }

        [Command("character")]
        [Alias("char", "c")]
        [Summary("Gets a character from name.")]
        public async Task CharacterAsync([Remainder]string name)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetCharacterAsync(name);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }

        [Command("staff")]
        [Alias("s")]
        [Summary("Gets a character from an id.")]
        public async Task UserAsync([Remainder]string query)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetStaffAsync(query);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }

        [Command("staff")]
        public async Task UserAsync([Remainder]long id)
        {
            AnilistClient client = new AnilistClient();
            var ch = await client.GetStaffAsync(id);
            await ReplyAsync(embed: AnilistHelper.BuildEmbed(ch));
        }
    }
}
