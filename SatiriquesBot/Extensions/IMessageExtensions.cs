using System;
using System.Collections.Generic;
using System.Text;

namespace Discord
{
    public static class IMessageExtensions
    {
        public static string GetLink(this IMessage message)
        {
            var chan = message.Channel as ITextChannel;

            return $"https://discordapp.com/channels/{chan.GuildId}/{chan.Id}/{message.Id}";
        }
    }
}
