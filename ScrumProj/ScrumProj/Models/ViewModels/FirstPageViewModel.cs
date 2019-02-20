using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models.ViewModels
{
    public class FirstPageViewModel
    {
        public ProfileModel loggedInUser;
        public Entry entry { get; set; }
        public List<EntryViewModel> ListOfEntriesToLoopInBlogView { get; set; }

        public File File { get; set; }
        public string comment { get; set; }
        public int CommentId { get; set; }
        public List<Comment> ListOfComments { get; set; }
        public List<Categories> Categories { get; set; }
        public List<CategoryInEntry> CategoryIds { get; set; }
        public List<Meeting> MeetingsOfUser { get; set; }
        public List<DevelopmentProject> DevsOfUser { get; set; }
    }
}