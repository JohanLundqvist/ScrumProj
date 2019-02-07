using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class File
    {
        public int FileId { get; set; }
        [Display (Name="Filnamn")]
        public string FileName { get; set; }
        public byte[] FileBytes { get; set; }
    }
}