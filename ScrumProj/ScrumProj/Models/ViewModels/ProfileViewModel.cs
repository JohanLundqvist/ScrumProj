﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Models
{
    public class ProfileViewModel
    {
        public string ID { get; set; }

        [Required(ErrorMessage = "Ange ditt förnamn")]
        [StringLength(60, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Ange ditt efternamn")]
        [StringLength(60, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Ange din befattning")]
        [StringLength(60, MinimumLength = 1)]
        public string Position { get; set; }

        public bool Exist { get; set; }

        public List<Categories> Categories { get; set; }

        public List<ProfileModel> Profiles { get; set; }

        public bool NewPushNote { get; set; }

        public List<PushNote> PushNotes { get; set; }

        public WantMailOrNo WantMailOrNo { get; set; }
    }
}
