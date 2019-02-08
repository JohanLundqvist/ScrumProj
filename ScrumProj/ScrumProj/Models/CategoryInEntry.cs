using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class CategoryInEntry
    {
        public int Id { get; set; }
        public int EntryId { get; set; }
        public int CategoryId { get; set; }
    }
}