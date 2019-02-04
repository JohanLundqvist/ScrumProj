using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class EntryViewModel
    {
        public ProfileModel loggedInUser;
        public Entry entry { get; set; }
        public List<EntryViewModel> ListOfEntriesToLoopInBlogView { get; set; }
        public File File { get; set; }
        public string comment { get; set; }
        public int CommentId { get; set; }
        public List<Comment> ListOfComments { get; set; }
    }
}