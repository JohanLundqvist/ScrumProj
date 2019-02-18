using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class WantMailOrNo
    {
        public int Id { get; set; }
        public bool BlogPost { get; set; }
        public bool Mail { get; set; }
        public bool Sms { get; set; }
        public bool Project { get; set; }
        public string UserId { get; set; }
    }
}