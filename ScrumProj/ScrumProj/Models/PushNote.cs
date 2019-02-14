using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class PushNote
    {
        public int Id { get; set; }
        public string Note { get; set; }
        public string ProfileModelId { get; set; }
        public string TypeOfNote { get; set; }
    }
}