using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Entities;

namespace SatiriquesBot.Database.Controllers
{
    public class ReminderController : DbController<ReminderContext>
    {
        public ReminderController(ReminderContext db) : base(db)
        {
        }

        public async Task AddTimedReminderAsync(SocketCommandContext context,
                                                TimeSpan delay,
                                                TimeSpan interval,
                                                string message = null,
                                                CallerDelegate callerDelegate = CallerDelegate.None)
        {
            var remainder = new Reminder
            {
                Creation = DateTime.Now,
                CreatedBy = context.User.Id,
                GuildId = context.Guild.Id,
                CallerDelegate = callerDelegate,
                ChannelId = context.Channel.Id,
                Delay = delay,
                RepeatIndefinitely = false,
                NumberOfRepeats = null,
                Interval = interval,
                Message = message
            };

            await _db.Remainders.AddAsync(remainder);
            await _db.SaveChangesAsync();
        }

        public async Task<Reminder> GetLastReminderAsync()
        {
            return await _db.Remainders.LastOrDefaultAsync();
        }

        public async Task DeleteRemainderAsync(ulong id)
        {
            var entity = await _db.Remainders.FirstOrDefaultAsync(x => x.Id == id);

            if (entity != default)
            {
                _db.Remove(entity);
                await _db.SaveChangesAsync();
            }
        }

        public async Task<Reminder[]> GetRemindersAsync()
        {
            return await _db.Remainders.ToArrayAsync();
        }
    }
}