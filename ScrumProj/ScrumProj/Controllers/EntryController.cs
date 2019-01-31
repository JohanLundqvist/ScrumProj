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
    public class EntryController : Controller
    {
        
        // GET: Entry
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult EntryView(EntryViewModel model) {

            

            return View(model);
        }

        public ActionResult PublishEntry(EntryViewModel model) {

            var ctx = new AppDbContext();
            model.loggedInUser = GetCurrentUser(User.Identity.GetUserId());
            var UserId = model.loggedInUser.ID;
            ctx.Entries.Add(new Entry {
                AuthorId = UserId,
                Content = model.content.Content
            });


            return View("Blogginlägg");
        }

        public ProfileModel GetCurrentUser(string Id)
        {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }
    }
}