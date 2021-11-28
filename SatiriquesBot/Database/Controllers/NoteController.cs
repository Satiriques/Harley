using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Microsoft.EntityFrameworkCore;
using SatiriquesBot.Database.Contexts;
using SatiriquesBot.Database.Entities;

namespace SatiriquesBot.Database.Controllers
{
    public class NoteController : DbController<NoteContext>
    {
        public NoteController(NoteContext db) : base(db)
        {
        }

        public async Task<NoteGroup> GetGroupAsync(string noteName) =>
            await AsyncEnumerable.FirstOrDefaultAsync(_db.NoteGroups, x =>
                x.Name.Equals(noteName, StringComparison.OrdinalIgnoreCase));

        public async Task<bool> AddNoteGroupAsync(string groupName, IUser creator)
        {
            if (await GroupNoteExistsAsync(groupName)) return false;
            await _db.NoteGroups.AddAsync(new NoteGroup(groupName, creator.Id));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> GroupNoteExistsAsync(string noteName) =>
            await AsyncEnumerable.AnyAsync(_db.NoteGroups,
                x => x.Name.Equals(noteName, StringComparison.OrdinalIgnoreCase));

        public async Task<NoteGroup[]> GetGroupsAsync() => await AsyncEnumerable.ToArrayAsync(_db.NoteGroups);

        public async Task<bool> AddNoteAsync(string groupName, string noteName, string message, IUser creator)
        {
            if (!await GroupNoteExistsAsync(groupName) || await NoteExistsAsync(groupName, noteName))
                return false;

            await _db.Notes.AddAsync(new Note(await GetGroupAsync(groupName), noteName, creator.Id, message));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> NoteExistsAsync(string groupName, string noteName) =>
            await AsyncEnumerable.AnyAsync(_db.Notes,
                x => x.NoteGroup.Name.Equals(groupName) && x.Name.Equals(noteName));

        public async Task<Note[]> GetNotesAsync(string groupName) => await _db.Notes.AsQueryable()
                                                                              .Where(x =>
                                                                                  x.NoteGroup.Name.Equals(groupName))
                                                                              .ToArrayAsync();

        public async Task<Note> GetNoteAsync(string groupName, string noteName) =>
            await _db.Notes.AsQueryable().FirstOrDefaultAsync(x =>
                x.NoteGroup.Name == groupName.ToLower() &&
                x.Name == noteName.ToLower());

        public async Task<bool> DeleteNoteAsync(string groupName, string noteName)
        {
            var note = await GetNoteAsync(groupName, noteName);
            if (note == null) return false;
            _db.Notes.Remove(note);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteGroupAsync(string groupName)
        {
            var group = await GetGroupAsync(groupName);
            if (group == null) return false;

            var notes = await GetNotesAsync(groupName);
            _db.NoteGroups.Remove(group);
            _db.Notes.RemoveRange(notes);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task UpdateNoteAsync(Note note, IUser user)
        {
            note.Modification = DateTime.Now;
            note.LastModifiedBy = user.Id;

            _db.Notes.Update(note);
            await _db.SaveChangesAsync();
        }
    }
}