using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        

        [Display(Name = "EntryImage")]
        public byte[] Image { get; set; }
    }
}