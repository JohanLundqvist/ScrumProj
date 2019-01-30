using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class File
    {
        public int FileId { get; set; }
        public int Name { get; set; }
        public byte[] FileBytes { get; set; }
    }
}