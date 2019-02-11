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
        [StringLength (30)]
        public string Title { get; set; }

        [StringLength(1000)]
        [Display(Name = "Inledning" )]
        public string Content { get; set; }

        [Required]
        public virtual ICollection<ProfileModel> Participants { get; set; }

        public enum Category
        {
            Forskning,
            Utbildning
        }
        public File UploadedFile { get; set; }
        [Display(Name = "Kategori")]
        public Category Cat { get; set; }

    }
}