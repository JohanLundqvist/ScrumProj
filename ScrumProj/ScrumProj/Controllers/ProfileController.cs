﻿using Microsoft.AspNet.Identity;
using ScrumProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class ProfileController : Controller
    {
        /*
        // GET: Profile
        public ActionResult Index()
        {
            var ctx = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var userProfile = ctx.Profiles.FirstOrDefault();

            var exist = false;

            if (userProfile != null)
            {
                exist = true;
            }

            var vm = new ProfileViewModel
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Position = userProfile.Position,
                Exist = exist
            };

            return View(vm);
        }



        // Method to create a profile
        public ActionResult CreateProfile(ProfileViewModel model)
        {
            var ctx = new ApplicationDbContext();
            var currentUserId = User.Identity.GetUserId();
            var userProfile = ctx.Profiles.FirstOrDefault();

            if (userProfile == null)
            {
                ctx.Profiles.Add(new ProfileModel
                {

                });
            }

            return RedirectToAction("Index");
        }
        */
    }
}