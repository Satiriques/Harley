using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SatiriquesBot.Database.Controllers;
using SatiriquesBot.Database.Entities;
using SatiriquesBot.Services;

namespace SatiriquesBot.Modules.Utils
{
    public class ReminderModule : ModuleBase<SocketCommandContext>
    {
        private readonly ReminderService _reminderService;

        public ReminderModule(ReminderService reminderManager)
        {
            _reminderService = reminderManager;
        }
        
        [Command("addreminder")]
        [Alias("ar")]
        public async Task AddRemainderAsync(TimeSpan delay, TimeSpan interval, CallerDelegate callerDelegate, [Remainder] string message)
        {
            await _reminderService.AddTimedReminderAsync(Context, delay, interval, message, callerDelegate);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        [Command("adddailyreminder")]
        [Alias("adr")]
        public async Task AddDailyReminderAsync(DateTime start, [Remainder]string message)
        {
            if (start < DateTime.Now)
            {
                start = start.AddDays(1);
            }

            var delay = start - DateTime.Now;
            await _reminderService.AddTimedReminderAsync(Context, delay, TimeSpan.FromDays(1), message, CallerDelegate.None);
            await ReplyAsync($"The first message will be sent the : {start}");
        }

        [Command("listreminders")]
        [Alias("lsr")]
        public async Task ListRemindersAsync()
        {
            var remainders = await _reminderService.GetRemindersAsync();
            await ReplyAsync(Format.Code(string.Join(Environment.NewLine, FormatReminder(remainders))));
        }

        [Command("deletereminder")]
        [Alias("dr")]
        public async Task DeleteReminderAsync(ulong id)
        {
            await _reminderService.DeleteReminderAsync(id);
            await Context.Message.AddReactionAsync(new Emoji("✅"));
        }

        public IEnumerable<string> FormatReminder(Reminder[] remainders)
        {
            yield return nameof(Reminder.Id) + "\t" + nameof(Reminder.Delay) + "\t" + nameof(Reminder.Interval) + "\t" + nameof(Reminder.CallerDelegate) + "\t" + nameof(Reminder.Message);

            foreach (var remainder in remainders.Select(x =>
                x.Id + "\t" + x.Delay + "\t" + x.Interval + "\t" + x.CallerDelegate + "\t" + x.Message))
            {
                yield return remainder;
            }
        }
    }
}