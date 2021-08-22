using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class Note
    {
        [Key]
        public int NoteId { get; set; }

        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(400)")]
        public string Text { get; set; }

        public string CreatedOn { get; set; }

        public int GroupId { get; set; }
        [JsonIgnore]
        public Group Group { get; set; }
    }
}
