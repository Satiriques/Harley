using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Miki.Anilist;

namespace SatiriquesBot.Modules.Anilist.Helper
{
    public static class AnilistHelper
    {
        internal static Embed BuildEmbed(IMediaSearchResult mediaSearchResult)
        {
            var media = mediaSearchResult as IMedia;

            return new EmbedBuilder()
            {
                Title = media.DefaultTitle,
                ThumbnailUrl = media.CoverImage,
                Description = media.Description,
                Url = media.Url
            }.Build();
        }

        internal static Embed BuildEmbed(ICharacterSearchResult characterSearchResult)
        {
            var ch = characterSearchResult as ICharacter;

            return new EmbedBuilder()
            {
                Title = ch.LastName + ", " + ch.FirstName,
                ThumbnailUrl = ch.LargeImageUrl,
                Description = ch.Description.Replace("~!", "||").Replace("!~", "||"),
                Url = ch.SiteUrl
            }.Build();
        }
    }
}
