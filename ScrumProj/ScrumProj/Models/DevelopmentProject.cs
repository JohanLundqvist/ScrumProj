using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class DevelopmentProject
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Titel")]
        [StringLength(30)]
        public string Title { get; set; }

        [StringLength(1000)]
        [Display(Name = "Inledning")]
        public string Content { get; set; }

        [Display(Name = "Kollegor")]
        public virtual ICollection<ProfileModel> Participants { get; set; }

        [Display(Name = "Filer")]
        public virtual ICollection<DevFile> Files { get; set; }

        public enum Category
        {
            Forskning,
            Utbildning
        }
        

        public enum IsPrivate
        {
            Publik,  
            Privat
        }
        [Display(Name = "Synlighet")]
        public IsPrivate Visibility { get; set; }

        [Display(Name = "Kategori")]
        public Category Cat { get; set; }

        public DevelopmentProject(){
            this.Participants = new HashSet<ProfileModel>();

            }

    }
}