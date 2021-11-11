using Discord;
using SatiriquesBot.Database.Controllers;
using SatiriquesBot.Database.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interactivity;

namespace Discord
{
    public static class IUserExtensions
    {
        private static UserController _db;
        public static void Configure(UserController controller)
        {
            _db = controller;
        }
        public static Embed BuildEmbed(this IUser discordUser, bool isSelf=false)
        {
            return new EmbedBuilder()
            {
                Title = discordUser.Username,
                ThumbnailUrl = discordUser.GetAvatarUrl()
            }.Build();
        }

        public static Page BuildProfilePage(this IUser discordUser, bool isSelf = false)
        {
            return new PageBuilder()
            {
                Title = discordUser.Username,
                ThumbnailUrl = discordUser.GetAvatarUrl()
            }.Build();
        }

        private static Page[] BuildPages(UserPage[] pages, IUser user)
        {
            var paginatedPages = new List<Page>()
            {
                BuildProfilePage(user)
            };

            foreach(var userPage in pages)
            {
                paginatedPages.Add(userPage.Page);
            }
            return paginatedPages.ToArray();
        }

        // public static async Task<PaginatedMessage> BuildProfileAsync(this IUser discordUser)
        // {
        //     var dbUser = await _db.GetUserAsync(discordUser);
        //
        //     return new PaginatedMessage()
        //     {
        //         Title = discordUser.Username,
        //         Color = Color.Teal,
        //         Pages = BuildPages(dbUser.Pages.ToArray(), discordUser),
        //         Author = new EmbedAuthorBuilder()
        //         {
        //             IconUrl = discordUser.GetAvatarUrl(),
        //             Name = discordUser.Username
        //         }
        //     };
        // }
    }
}
