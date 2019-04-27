using Discord;
using Discord.Addons.Interactive;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using static Discord.Addons.Interactive.PaginatedMessage;

namespace SatiriquesBot.Database.Entities
{
    public enum TemplateType
    {
        None,
        [Description("leagueofcomicgeeks.com")]
        LeagueOfComicGeeks,
        [Description("anilist.co")]
        AniList,
        [Description("myanimelist.net")]
        MyAnimeList,
        [Description("www.manga-sanctuary.com")]
        MangaSanctuary,
        [Description("mangadex.org")]
        MangaDex
    }
    public class UserPage : IEntity<ulong>
    {
        public ulong Id { get; set; }
        public ulong DiscordId { get; set; }
        public TemplateType TemplateType { get; set; }

        public string Url { get; set; }
        [NotMapped]
        public Page Page { get => PageFromTemplate(); }

        private Page PageFromTemplate()
        {
            switch (TemplateType)
            {
                case TemplateType.LeagueOfComicGeeks:
                    return new Page()
                    {
                        Title = "League of Comic Geeks",
                        ThumbnailUrl = "https://pbs.twimg.com/profile_images/463177498226749440/0MsEClY-.png",
                        Color = Color.Orange,
                        Url = Url,
                        Description = "Join the ultimate social network for comic book fans that makes it easy to track your comic book collection or pull list and stay on top of the hottest new releases."
                    };
                case TemplateType.AniList:
                    return new Page()
                    {
                        Title = "AniList",
                        ThumbnailUrl = "https://anilist.co/img/icons/android-chrome-512x512.png",
                        Color = new Color(2, 169, 255),
                        Url = Url,
                        Description = "Create an AniList account today! Sign Up. Site Theme. A. A. A. AniList.co AniChart.net Apps Discord Twitter Facebook GitHub API Contact Terms & Privacy.",
                    };
                case TemplateType.MyAnimeList:
                    return new Page()
                    {
                        Title = "MyAnimeList",
                        ThumbnailUrl = "https://i.imgur.com/GAAvTGu.png",
                        Color = new Color(46, 81, 162),
                        Url = Url,
                        Description = "Welcome to MyAnimeList, the world's most active online anime and manga community and database."
                    };
                case TemplateType.MangaSanctuary:
                    return new Page()
                    {
                        Title = "Manga Sanctuary",
                        ThumbnailUrl = "https://pbs.twimg.com/profile_images/977145638/toori.jpg",
                        Color = new Color(255,196,1),
                        Url = Url,
                        Description = "Manga Sanctuary : base de données d'infos sur les manga, manhwa, manhua, DVD, Blu-ray, japanimation mais aussi un planning des sorties complet, un ..."
                    };
                case TemplateType.MangaDex:
                    return new Page()
                    {
                        Title = "MangaDex",
                        ThumbnailUrl = "https://mangaplanet.jp/wp-content/uploads/2018/09/90B8EAA6-5BF9-42DE-A53D-28CB578D6280.png",
                        Color = new Color(247,148,33),
                        Url = Url,
                        Description = "Read manga online for free at MangaDex with no ads, high quality images and support scanlation groups!"
                    };
                default:
                    return null;
            }
        }
    }
}
