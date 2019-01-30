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

        [Display (Name = "Inlägg")]
        [StringLength(1000, ErrorMessage ="Inlägget får endast innehålla 1000 tecken!")]
        public string Content { get; set; }
        public string AuthorId { get; set; }
    }
}