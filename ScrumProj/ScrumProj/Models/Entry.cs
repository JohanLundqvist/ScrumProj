using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ScrumProj.Models
{
    public class Entry
    {
        public int Id { get; set; }
        
        [Display(Name = "Inlägg")]
        [Required(ErrorMessage = "Vänligen fyll i Innehållet" )]
        [StringLength(1000, ErrorMessage = "Inlägget får endast innehålla 1000 tecken!")]
        public string Content { get; set; }

        public string AuthorId { get; set; }

        [Display(Name = "Titel")]
        [Required(ErrorMessage = "Vänligen fyll i en Titel")]
        public string Title { get; set; }

        public int fileId { get; set; }
        public string Author { get; set; }
        public bool Formal { get; set; }
        public string ImageUrl { get; set; }
    }
}
