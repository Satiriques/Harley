using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using SatiriquesBot.Database.Entities;

namespace SatiriquesBot.Database.Contexts
{
    public sealed class NoteContext : DbContext
    {
        public DbSet<Note> Notes { get; set; }
        public DbSet<NoteGroup> NoteGroups { get; set; }

        public NoteContext()
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

            builder.UseSqlite($"Filename={Path.Combine(dir, "notes.db")}");
        }
    }
}