using Discord;
using System.ComponentModel.DataAnnotations.Schema;

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
