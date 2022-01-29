using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;

namespace SatiriquesBot.Database.Entities
{
    public enum CallerDelegate
    {
        None,
    }

    public class Reminder : IEntity<ulong>
    {
        public Reminder()
        {
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        [Required] public ulong CreatedBy { get; set; }
        [Required] public DateTime Creation { get; set; }
        [Required] public ulong GuildId { get; set; }
        [Required] public ulong ChannelId { get; set; }

        [Required] public bool RepeatIndefinitely { get; set; }
        [Required] public TimeSpan Interval { get; set; }
        [Required] public TimeSpan Delay { get; set; }
        public int? NumberOfRepeats { get; set; }
        [Required] public CallerDelegate CallerDelegate { get; set; }
        public string Message { get; set; }
    }
}