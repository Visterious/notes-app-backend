using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }

        [Column(TypeName = "nvarchar(40)")]
        public string Name { get; set; }

        public int CreatorId { get; set; }

        public int CountOfUsers { get; set; }

        [JsonIgnore]
        public ICollection<Note> Notes { get; set; }

        //[JsonIgnore]
        public ICollection<User> Users { get; set; }

        public Group()
        {
            Notes = new List<Note>();
            Users = new List<User>();
        }
    }
}
