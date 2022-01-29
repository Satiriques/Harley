using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using SatiriquesBot.Database.Controllers;
using SatiriquesBot.Database.Entities;

namespace SatiriquesBot.Services
{
    public class ReminderService
    {
        private readonly ReminderController _db;
        private readonly Dictionary<ulong, Timer> _timersDictionary = new();
        private DiscordSocketClient _client;

        public ReminderService(ReminderController db)
        {
            _db = db;
        }

        public async Task StartAsync(DiscordSocketClient client)
        {
            _client = client;

            var reminders = await _db.GetRemindersAsync();

            foreach (var reminder in reminders)
            {
                if (!string.IsNullOrWhiteSpace(reminder.Message))
                {
                    var timer = CreateTimer(reminder);

                    if (timer == null)
                        continue;

                    _timersDictionary.Add(reminder.Id, timer);
                }
            }

            await Task.CompletedTask;
        }

        private Timer CreateTimer(Reminder reminder)
        {
            var guild = _client.GetGuild(reminder.GuildId);

            var channel = guild?.GetTextChannel(reminder.ChannelId);

            return channel == null
                ? null
                : new Timer(_ => channel.SendMessageAsync(reminder.Message), null, reminder.Delay, reminder.Interval);
        }


        public async Task AddTimedReminderAsync(SocketCommandContext context, TimeSpan delay, TimeSpan interval,
                                                string message, CallerDelegate callerDelegate)
        {
            await _db.AddTimedReminderAsync(context, delay, interval, message, callerDelegate);
            var reminder = await _db.GetLastReminderAsync();

            var timer = CreateTimer(reminder);

            if (timer == null)
                return;

            _timersDictionary.Add(reminder.Id, timer);
        }

        public async Task<Reminder[]> GetRemindersAsync()
        {
            return await _db.GetRemindersAsync();
        }

        public async Task DeleteReminderAsync(ulong id)
        {
            if (!_timersDictionary.ContainsKey(id))
                return;

            var reminder = _timersDictionary[id];
            await reminder.DisposeAsync();
            
            _timersDictionary.Remove(id);
            await _db.DeleteRemainderAsync(id);
        }
    }
}