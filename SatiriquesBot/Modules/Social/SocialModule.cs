using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SatiriquesBot.Database.Controllers;
using System;
using SatiriquesBot.Database.Entities;
using System.Linq;

namespace SatiriquesBot.Modules.Social
{
    public class SocialModule : ModuleBase<SocketCommandContext>
    {
        private UserController _db;
        public SocialModule(UserController controller)
        {
            _db = controller;
        }


        [Command("profile", RunMode = RunMode.Async)]
        public async Task SocialModuleAsync()
        {
            // await PagedReplyAsync(await Context.User.BuildProfileAsync(), new ReactionList());

        }

        [Command("profile")]
        public async Task SocialModuleAsync(IUser user)
        {
            // await PagedReplyAsync(await user.BuildProfileAsync(), new ReactionList());
        }

        [Command("addpage")]
        public async Task AddPageAsync(string url) 
        {
            Uri uri = new Uri(url);
            TemplateType type = TemplateType.None;

            type = EnumExtensions.GetValueFromDescription<TemplateType>(uri.Host);

            await _db.AddPageAsync(Context.User, type, url);
        }

        [Command("stalk")]
        public async Task StalkAsync(IUser user, ITextChannel channel = null)
        {
            ITextChannel chan = channel ?? Context.Channel as ITextChannel;
            var messages = (await chan.GetMessagesAsync(1000).FlattenAsync())
                              .Where(x => x.Author == user)
                              .Take(10)
                              .Reverse()
                              .Select(x=> $"[link]({x.GetLink()}) {Format.Code(x.Content.ClearCode())}");

            await ReplyAsync(embed: new EmbedBuilder() {
                Description = string.Join(Environment.NewLine, messages),
            }.Build());
           
        }

        [Command("stalk")]
        public async Task StalkAsync(string query, ITextChannel channel = null)
        {
            ITextChannel chan = channel ?? Context.Channel as ITextChannel;
            var messages = (await chan.GetMessagesAsync(100000).FlattenAsync())
                              .Where(x => x.Content.ToLower().Contains(query.ToLower()))
                              .Take(10)
                              .Reverse()
                              .Select(x => $"[link]({x.GetLink()}) {Format.Code(x.Content.ClearCode())}");

            await ReplyAsync(embed: new EmbedBuilder()
            {
                Description = string.Join(Environment.NewLine, messages),
            }.Build());

        }


    }
}
