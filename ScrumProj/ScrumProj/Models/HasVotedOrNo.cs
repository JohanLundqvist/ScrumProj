using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class HasVotedOrNo
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MeetingId { get; set; }
        public bool Hasvoted { get; set; }

    }
}