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
        public File File { get ; set; }
    }
}