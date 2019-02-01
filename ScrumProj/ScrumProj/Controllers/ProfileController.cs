using Microsoft.AspNet.Identity;
using ScrumProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        public ActionResult Index()
        {
            var ctx = new AppDbContext();
            var currentUserId = User.Identity.GetUserId();
            var userProfile = ctx.Profiles.FirstOrDefault(p => p.ID == currentUserId);
            
            var exist = false;

            if (userProfile != null)
            {
                exist = true;
            }

            var vm = new ProfileViewModel
            {
                FirstName = userProfile?.FirstName,
                LastName = userProfile?.LastName,
                Position = userProfile?.Position,
                Exist = exist
            };

            return View(vm);
        }
        // Method to create a profile
        [HttpPost]
        public ActionResult CreateProfile(ProfileViewModel model)
        {
            var ctx = new AppDbContext();
            var currentUserId = User.Identity.GetUserId();
            var userProfile = ctx.Profiles.FirstOrDefault(p => p.ID == currentUserId);

            if (userProfile == null)
            {
                ctx.Profiles.Add(new ProfileModel
                {
                    ID = currentUserId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Position = model.Position
                });
            }

            ctx.SaveChanges();

            return RedirectToAction("Index");
        }
        

        public ProfileModel GetCurrentUser(string Id) {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }
    }
}