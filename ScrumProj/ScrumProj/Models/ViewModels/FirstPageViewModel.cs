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

        [ValidateFileSize(ErrorMessage = "Invalid File")]
        public File File { get; set; }
        public string comment { get; set; }
        public int CommentId { get; set; }
        public List<Comment> ListOfComments { get; set; }
        public List<Categories> Categories { get; set; }
        public List<CategoryInEntry> CategoryIds { get; set; }
    }
}