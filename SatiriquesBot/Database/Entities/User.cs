using Discord;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using static Discord.Addons.Interactive.PaginatedMessage;

namespace SatiriquesBot.Database.Entities
{
    public class User : IEntity<ulong>
    {
        public ulong Id { get; set; }
        public ulong DiscordId { get; set; }

        [NotMapped]
        public UserPage[] Pages { get; set; }

        public User(ulong discordId)
        {
            DiscordId = discordId;
        }
    }
}
