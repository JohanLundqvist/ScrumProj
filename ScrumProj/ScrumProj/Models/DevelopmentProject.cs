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

        [Required(ErrorMessage = "En titel behövs för att kunna publicera projeketet.")]
        [Display(Name = "Titel")]
        [Required(ErrorMessage = "Du måste ha en titel på ditt projekt.")]
        [StringLength(30,ErrorMessage ="Max 30 tecken.")]
        public string Title { get; set; }

        [Required(ErrorMessage ="En inledning behövs för att kunna publicera projeketet.")]
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
            this.Files = new HashSet<DevFile>();

            }

    }
}