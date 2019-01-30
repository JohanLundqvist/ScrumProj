using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class Entry
    {
        public int Id { get; set; }
        [Display (Name= "Inlägg")]
        [StringLength(1000, ErrorMessage = "Inlägget får inte överskrida 1000 tecken")]
        public string content { get; set; }
        public string AuthorId { get; set; }
        
    }
}