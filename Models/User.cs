using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(30)")]
        public string Username { get; set; }

        public string Password { get; set; }
        [JsonIgnore]
        public ICollection<Group> Groups { get; set; }

        public User()
        {
            Groups = new List<Group>();
        }
    }
}
