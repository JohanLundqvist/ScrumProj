using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class ProfileModel
    {
        [Key]
        public string ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public bool IsApproved { get; set; }
        public virtual ICollection<DevelopmentProject> Projects { get; set; }
        public bool NewPushNote { get; set; }

        public ProfileModel(){
            this.Projects = new HashSet<DevelopmentProject>();
        }
    }
}