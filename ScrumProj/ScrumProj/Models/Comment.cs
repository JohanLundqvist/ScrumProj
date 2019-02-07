using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string comment { get; set; }
        public int EntryId { get; set; }
        public string Author { get; set; }
    }
}