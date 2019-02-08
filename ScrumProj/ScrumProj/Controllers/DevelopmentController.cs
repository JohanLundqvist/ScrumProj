using ScrumProj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumProj.Models;
using Microsoft.AspNet.Identity;

namespace ScrumProj.Controllers
{
    public class DevelopmentController : Controller
    {
        private AppDbContext _context { get; set; } 
        // GET: Development
        [Authorize]
        public ActionResult DevelopmentWork(DevelopmentViewModel model)
        {
            model = FillModel();

                return View(model);
        }

        [Authorize]
        public ActionResult PublishDevProject(DevelopmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var proj = new DevelopmentProject
                {
                    Title = model.project.Title,
                    Content = model.project.Content,
                    Cat = model.project.Cat
                    
                };
            }
            model = FillModel();
            return View("DevelopmentWork", model);
        }

        protected DevelopmentViewModel FillModel()
        {
            DevelopmentViewModel model = new DevelopmentViewModel();
            _context = new AppDbContext();
            var listOfUsers = new List<ProfileModel>();
            var listOfProjects = new List<DevelopmentProject>();
            var activeUser = new ProfileModel();

            foreach(var proj in _context.Projects)
            {
                listOfProjects.Add(proj);
            }
            foreach(var user in _context.Profiles)
            {
                if (!user.ID.Equals(User.Identity.GetUserId()))
                {
                    listOfUsers.Add(user);
                }
                
            }

            var idToCompare = User.Identity.GetUserId();

            activeUser = _context.Profiles.SingleOrDefault(u => u.ID == idToCompare);

            model.projects = listOfProjects;
            model.Users = listOfUsers;
            model.ActiveUser = activeUser;

            return model;
        }
    }
}