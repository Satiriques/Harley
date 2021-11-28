using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Interactivity;
using SatiriquesBot.Database.Controllers;

namespace SatiriquesBot.Modules.Note
{
    public class NoteModule : ModuleBase<SocketCommandContext>
    {
        private readonly NoteController _db;
        private readonly InteractivityService _interactivityService;

        public NoteModule(NoteController db, InteractivityService interactivityService)
        {
            _interactivityService = interactivityService;
            _db = db;
        }

        [Command("addgroup")]
        [Alias("ag")]
        public async Task AddGroup(string groupName)
        {
            var success = await _db.AddNoteGroupAsync(groupName, Context.User);
            await Context.Message.AddReactionAsync(success ? new  Emoji("✅") :new  Emoji("❌") );
        }

        [Command("list")]
        [Alias("ls")]
        public async Task ListGroups()
        {
            var groups = await _db.GetGroupsAsync();
            await ReplyAsync(
                $"{Format.Bold("Groups:")}{Environment.NewLine}{Format.Code(string.Join(",", groups.OrderBy(x => x.Name).Select(x => x.Name)))}");
        }

        [Command("addnote")]
        [Alias("an")]
        public async Task AddNote(string groupName, string noteName, [Remainder] string message)
        {
           var success = await _db.AddNoteAsync(groupName, noteName, message, Context.User);
            await Context.Message.AddReactionAsync(success ? new  Emoji("✅") :new  Emoji("❌") );
        }

        [Command("list")]
        [Alias("ls")]
        public async Task ListNotes(string groupName)
        {
            var notes = await _db.GetNotesAsync(groupName);
            await ReplyAsync(
                $"{Format.Bold($"Notes in group {Format.Code(groupName)}:")}{Environment.NewLine}{Format.Code(string.Join(",", notes.OrderBy(x => x.Name).Select(x => x.Name)))}");
        }

        [Command("note")]
        [Alias("n")]
        public async Task Note(string groupName, string noteName)
        {
            var note = await _db.GetNoteAsync(groupName, noteName);

            var user = Context.Guild.GetUser(note.CreatedBy);
            if (note != null)
            {
                var modif = note.LastModifiedBy == 0 ? string.Empty : $" - Last Modif: ({Context.Guild.GetUser(note.LastModifiedBy)?.Username ?? note.LastModifiedBy.ToString()} - {note.Modification:dd-MM-yy})"; 
                await ReplyAsync(embed: new EmbedBuilder
                    {
                        Description = note.Message, 
                        Title = groupName + " - " + noteName,
                        Footer = new EmbedFooterBuilder
                        {
                            Text = $"{user.Username ?? note.CreatedBy.ToString()} - {note.Creation:dd-MM-yy} {modif}",
                            IconUrl = user?.GetAvatarUrl()
                        }
                    }.Build());}
            else
                await Context.Message.AddReactionAsync(new Emoji("❓"));
        }

        [Command("removenote")]
        [Alias("rn")]
        public async Task RemoveNoteAsync(string groupName, string noteName)
        {
            var result = await _db.DeleteNoteAsync(groupName, noteName);
            await Context.Message.AddReactionAsync(result ? new Emoji("✅") : new Emoji("❓"));
        }

        [Command("removegroup")]
        [Alias("rg")]
        public async Task RemoveGroupAsync(string groupName)
        {
            var result = await _db.DeleteGroupAsync(groupName);
            await Context.Message.AddReactionAsync(result ? new Emoji("✅") : new Emoji("❓"));
        }

        [Command("editnote", RunMode = RunMode.Async)]
        [Alias("en")]
        public async Task EditNoteAsync(string groupName, string noteName)
        {
            var note = await _db.GetNoteAsync(groupName, noteName);

            if (note == null)
            {
                await Context.Message.AddReactionAsync(new Emoji("❓"));
                return;
            }

            await ReplyAsync(embed: new EmbedBuilder
            {
                Title = "Old Message:",
                Description = note.Message
            }.Build());

            await ReplyAsync("Please enter the new message:");
            var result = await _interactivityService.NextMessageAsync(x => x.Author == Context.User);

            if (result?.IsSuccess == true)
            {
                note.Message = result.Value.Content;
                await _db.UpdateNoteAsync(note, Context.User);
                await result.Value.AddReactionAsync(new Emoji("✅"));
            }
        }
    }
}