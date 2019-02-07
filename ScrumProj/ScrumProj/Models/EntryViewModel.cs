using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Models
{
    public class EntryViewModel
    {
        public ProfileModel loggedInUser;
        public Entry entry { get; set; }
        public List<EntryViewModel> ListOfEntriesToLoopInBlogView { get; set; }
        public List<EntryViewModel> ListOfInformalEntriesToLoopInBlogView { get; set; }

        [ValidateFileSize( ErrorMessage = "Invalid File")]
        public File File { get; set; }
        public string comment { get; set; }
        public int CommentId { get; set; }
        public List<Comment> ListOfComments { get; set; }


        [StringLength(maximumLength: 30, ErrorMessage = "Titeln får endast innehålla 30 tecken!")]
        public string Title { get; set; }
        public File File { get ; set; }
        

        [Display(Name = "EntryImage")]
        public byte[] Image { get; set; }
    }
    

}