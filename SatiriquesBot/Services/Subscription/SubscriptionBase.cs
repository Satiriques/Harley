using System.Net.Http;
using System.Threading.Tasks;
using Discord.WebSocket;
using SatiriquesBot.Services.Subscription.Content;

namespace SatiriquesBot.Services.Subscription
{
    public abstract class SubscriptionBase<T> : ISubscription
        where T : ContentBase
    {
        private readonly DiscordSocketClient _client;
        private readonly string _url;
        private static readonly HttpClient Client = new HttpClient();

        protected SubscriptionBase(DiscordSocketClient client, ulong guildId, ulong channelId, string url)
        {
            _client = client;
            _url = url;
            GuildId = guildId;
            ChannelId = channelId;
        }
        protected ulong GuildId { get; }
        protected ulong ChannelId { get; }

        public abstract Task UpdateWatcherAsync(T content);
        public abstract T AdaptContent(string urlResponse);

        protected async Task ReplyAsync(string message)
        {
            var guild = _client.GetGuild(GuildId);
            var channel = guild?.GetTextChannel(ChannelId);

            if (channel != null)
            {
                await channel.SendMessageAsync(message);
            }
        }
        
        public async Task CheckForUpdateAsync()
        {
            string response = null;

            try
            {
                response = await Client.GetStringAsync(_url);
            }
            catch
            {
                return;
            }

            if (!string.IsNullOrEmpty(response))
            {
                await UpdateWatcherAsync(AdaptContent(response));
            }
        }
    }

    public interface ISubscription
    {
        Task CheckForUpdateAsync();
    }
}