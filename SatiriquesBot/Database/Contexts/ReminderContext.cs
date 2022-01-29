using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SatiriquesBot.Database.Entities;

namespace SatiriquesBot.Database.Contexts
{
    public class ReminderContext : DbContext
    {
        public DbSet<Reminder> Remainders { get; set; }

        public ReminderContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string dir = Path.Combine(AppContext.BaseDirectory, "db");

            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            builder.UseSqlite($"Filename={Path.Combine(dir, "reminders.db")}");
        }
    }
}