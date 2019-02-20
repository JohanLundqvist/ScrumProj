using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScrumProj.Models
{
    public class MeetingTimes
    {
        public int Id { get; set; }
        public int MeetingId { get; set; }

        public string Time1 { get; set; }
        public int Time1Votes { get; set; }
        
        public string Time2 { get; set; }
        public int Time2Votes { get; set; }
        
        public string Time3 { get; set; }
        public int Time3Votes { get; set; }
        
        public string Time4 { get; set; }
        public int Time4Votes { get; set; }
    }
}