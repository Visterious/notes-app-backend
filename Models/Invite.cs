using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NotesAPI.Models
{
    public class Invite
    {
        [Key]
        public int InviteId { get; set; }

        public int InviterId { get; set; }

        public int InvitedId { get; set; }

        public int GroupId { get; set; }
    }
}
