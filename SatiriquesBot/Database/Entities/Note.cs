using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord;

namespace SatiriquesBot.Database.Entities
{
    public class NoteGroup : IEntity<ulong>
    {
        public NoteGroup()
        {
            
        }
        
        public NoteGroup(string name, ulong creatorId)
        {
            CreatedBy = creatorId;
            Name = name;
            Creation = DateTime.Now;
            Notes = new List<Note>();
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        [Required] public ICollection<Note> Notes { get; set; }
        [Required] public ulong CreatedBy { get; set; }
        public ulong LastModifiedBy { get; set; }
        [Required] public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }

        [Required] public string Name { get; set; }
    }

    public class Note : IEntity<ulong>
    {
        public Note()
        {
            
        }
        
        public Note(NoteGroup group, string name, ulong createdBy, string message)
        {
            NoteGroup = group;
            Name = name;
            CreatedBy = createdBy;
            Creation = DateTime.Now;
            Message = message;
        }

        [ForeignKey(nameof(NoteGroup))] public ulong GroupId { get; set; }
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        [Required] public NoteGroup NoteGroup { get; set; }

        [Required] public ulong CreatedBy { get; set; }
        public ulong LastModifiedBy { get; set; }

        [Required] public DateTime Creation { get; set; }
        public DateTime Modification { get; set; }

        [Required] public string Name { get; set; }
        [Required] public string Message { get; set; }
    }
}