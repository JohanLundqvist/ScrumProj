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
        private AppDbContext _context = new AppDbContext();

        [Authorize]
        public ActionResult DevelopmentWork(DevelopmentViewModel model)
        {
            var userId = User.Identity.GetUserId();
            var user = _context.Profiles.Single(u => u.ID == userId);
            model.ActiveUser = user;
            var listProj = new List<DevelopmentProject>();
            listProj = _context.Projects.ToList();
            model.projects = listProj;
            model.Users = _context.Profiles.ToList();


                return View(model);
        }

        [Authorize][HttpPost]
        
        public ActionResult PublishDevProject(DevelopmentViewModel model)
        {
            var idToCompare = User.Identity.GetUserId();
          var  activeUser = _context.Profiles.SingleOrDefault(u => u.ID == idToCompare);
            var partiList = new List<ProfileModel>();
            partiList.Add(activeUser);

            if (ModelState.IsValid)
            {
                _context.Projects.Add(new DevelopmentProject
                {
                    Title = model.project.Title,
                    Content = model.project.Content,
                    Cat = model.project.Cat,
                    Participants = partiList,
                    Visibility = model.project.Visibility
                });
              
                _context.SaveChanges();
            }
           return RedirectToAction("DevelopmentWork");
        }

        public ActionResult AddParticipants(DevelopmentViewModel model)
        { 

            var projectToUpdate = _context.Projects.First(p => p.Id == model.project.Id);

            if (projectToUpdate != null)
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Profiles.Single(u => u.ID == model.UserToAdd);
                    projectToUpdate.Participants.Add(user);
                    _context.SaveChanges();
                }
            }
            return RedirectToAction("EditDevelopmentPage", new { projectId = model.project.Id });
        }


        public ActionResult EditDevelopmentPage(int projectId, DevelopmentViewModel model)
        {
            var project = _context.Projects.First(p => p.Id == projectId);
            model.project = project;

            var listOfParti = project.Participants;
            var listOfNonMembers = new List<SelectListItem>();
           foreach(var user in _context.Profiles)
            {
                var anv = listOfParti.FirstOrDefault(x => x.ID == user.ID);
                if(anv is null)
                {
                    var item = new SelectListItem
                    {

                        Text = user.FirstName + " " + user.LastName,
                        Value = user.ID,
                        Selected = false
                    };
                    listOfNonMembers.Add(item);

                }
            }
            model.UsersFullName = listOfNonMembers;
            model.Users = _context.Profiles.ToList();

            return View(model);
        }

        public ActionResult Edit(DevelopmentViewModel model)
        {
            var projectToUpdate = _context.Projects.First(p => p.Id == model.project.Id);

            if (ModelState.IsValid)
            {
                projectToUpdate.Title = model.project.Title;
                projectToUpdate.Content = model.project.Content;
                projectToUpdate.Cat = model.project.Cat;
                _context.SaveChanges();
            }


            return RedirectToAction("DevelopmentWork");
        }
      
    }
}