using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Models.ViewModels
{
    public class MeetingViewModel
    {
        public Meeting Meeting { get; set; }
        public List<Meeting> Meetings { get; set; }
        public ProfileModel User { get; set; }
        public List<ProfileModel> Users { get; set; }
        public List<SelectListItem> UsersToAdd { get; set; }
        public string SelectedUser { get; set; }
        public List<string> ProposedTimes { get; set; }
        public MeetingTimes Times { get; set; }
        public Dictionary<string, double> DicTimes { get; set; }
        public HasVotedOrNo HasVotedOrNo { get; set; }

    }
}