using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class Meeting
    {   [Key]
        public int MeetingId { get; set; }
        [Required]
        [StringLength(30, ErrorMessage = "Mötet måste ha en titel.")]
        public string MeetingTitle { get; set; }
        public DateTime Time { get; set; }
        public List<DateTime> ProposedTimes { get; set; }
        public enum Status
        {
            Pending,
            Accepted
        }

        public Status State { get; set; }
        public virtual ICollection<ProfileModel> MeetingParticipants { get; set; }
        public Meeting()
        {
            this.MeetingParticipants = new HashSet<ProfileModel>();

        }
    }
}