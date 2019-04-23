using System;
using System.Collections.Generic;
using System.Text;

namespace SatiriquesBot.Database.Entities
{
    public class Page
    {
        public ulong Id { get; set; }
        public ulong DiscordId { get; set; }
        public string ThumbnailUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }

        public Page(string thumbnailUrl, string imageUrl, string description, string title)
        {
            ThumbnailUrl = thumbnailUrl;
            ImageUrl = imageUrl;
            Description = description;
            Title = title;
        }
    }
}
