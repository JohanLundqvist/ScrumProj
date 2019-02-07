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

        // required
        [Display (Name = "Inlägg")]
        [StringLength(maximumLength: 1000, ErrorMessage ="Inlägget får endast innehålla 1000 tecken!")]
        public string Content { get; set; }
        public string AuthorId { get; set; }
        [Display (Name ="Titel")]
        [Required]
        [StringLength(maximumLength: 30, ErrorMessage ="Titeln får endast innehålla 30 tecken!")]
        public string Title { get; set; }
        public int fileId { get; set; }
        public byte[] image { get; set; }
        public string Author { get; set; }
    }
    
}