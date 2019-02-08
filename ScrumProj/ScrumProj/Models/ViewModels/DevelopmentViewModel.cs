using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ScrumProj.Models;

namespace ScrumProj.Models.ViewModels
{
    public class DevelopmentViewModel
    {
        public DevelopmentProject project { get; set; }
        public List<DevelopmentProject> projects { get; set; }
        public List<ProfileModel> Users { get; set; }
        public ProfileModel ActiveUser { get; set; }

        public enum Category
        {
            Forskning,
            Utbildning
        }
        public string Content { get; set; }

        public string Title { get; set; }

        public List<ProfileModel> Participants { get; set; }

    }
}