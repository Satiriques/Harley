using System;
using System.Collections.Generic;
using System.Text;

namespace SatiriquesBot.Database.Entities
{
    public class User
    {
        public ulong Id { get; set; }
        public ulong DiscordId { get; set; }
        public List<Page> Pages { get; set; }

        public User(ulong discordId)
        {
            DiscordId = discordId;
        }
    }
}
