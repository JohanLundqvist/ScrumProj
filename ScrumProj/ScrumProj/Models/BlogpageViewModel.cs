using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class BlogpageViewModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public byte[] FileContent { get; set; }
        public string FileName { get; set; }
        public byte[] image { get; set; }
    }
}