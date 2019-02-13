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
        public virtual ProfileModel Profile { get; set; }
    }
}