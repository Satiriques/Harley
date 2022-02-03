using System;
using System.Collections.Generic;
using System.Timers;
using Discord.Commands;
using Discord.WebSocket;

namespace SatiriquesBot.Services.Subscription
{
    public class SubscriptionService
    {
        private readonly DiscordSocketClient _client;
        private readonly List<ISubscription> _subscriptions = new();
        private Timer _timer;

        public SubscriptionService(DiscordSocketClient client)
        {
            _client = client;
        }

        public void Start()
        {
            _timer = new Timer(TimeSpan.FromMinutes(15).TotalMilliseconds);
            _timer.Elapsed += (_, _) => _subscriptions.ForEach(SubscriptionCallbackAsync); 
            _timer.Start();
        }

        private async void SubscriptionCallbackAsync(ISubscription subscription)
        {
            await subscription.CheckForUpdateAsync();
        }

        public void Stop()
        {
            _timer.Stop();
        }
        
        public void AddSubscription<T>(SocketCommandContext commandContext, string url) where T : ISubscription
        {
            var instance = (T) Activator.CreateInstance(typeof(T),
                _client,
                commandContext.Guild.Id,
                commandContext.Channel.Id,
                url);
            
            _subscriptions.Add(instance);
        }
    }
}