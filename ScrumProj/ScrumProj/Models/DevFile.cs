using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class DevFile
    {
        [Key]
        public int FileId { get; set; }
        public byte[] Content { get; set; }
        public string Name { get; set; }
        public int ProjectId { get; set; }
        public DevelopmentProject ThisProject { get; set; }


    }
}