using ScrumProj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScrumProj.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

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

        [Authorize] [HttpPost]
        public ActionResult PublishDevProject(DevelopmentViewModel modell, HttpPostedFileBase upload)
        {
            var idToCompare = User.Identity.GetUserId();
            var activeUser = _context.Profiles.SingleOrDefault(u => u.ID == idToCompare);
            var partiList = new List<ProfileModel>();
            partiList.Add(activeUser);

            
                if (upload != null && upload.ContentLength > 0)
                {
                    var file = new DevFile
                    {
                        Name = System.IO.Path.GetFileName(upload.FileName)

                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        file.Content = reader.ReadBytes(upload.ContentLength);
                    }
                    modell.project.Files = new List<DevFile> { file };
                }

                _context.Projects.Add(new DevelopmentProject
                {
                    Title = modell.project.Title,
                    Content = modell.project.Content,
                    Cat = modell.project.Cat,
                    Participants = partiList,
                    Visibility = modell.project.Visibility,
                    Files = modell.project.Files
                });
                _context.SaveChanges();
            
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
                    NewPushNote(GetNameOfLoggedInUser() + " Har lagt till dig i projektet " + model.project.Title + "------" + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt"), user, "projectInvite");

                    var ap = new ApplicationDbContext();
                    List<string> Emails = new List<string>();
                    foreach (var p in ap.Users)
                    {
                        if (user.ID == p.Id)
                        {
                            Emails.Add(p.Email);
                        }
                    }
                    var s = GetNameOfLoggedInUser();
                    var mc = new MailController();
                    Task.Run(() => mc.SendEmail(new EmailFormModel
                    {
                        FromEmail = "scrumcgrupptvanelson@outlook.com",
                        FromName = "Nelson Administration",
                        Message = s + " Har lagt till dig i projektet " + model.project.Title + "------" + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt")
                    }, Emails));
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

        public void NewPushNote(string note, ProfileModel model, string typeOfNote)
        {
            var ctx = new AppDbContext();

            foreach (var p in ctx.Profiles)
            {
                if (p.ID == model.ID)
                {
                    var NewNote = new PushNote
                    {
                        Note = note,
                        ProfileModelId = p.ID,
                        TypeOfNote = typeOfNote

                    };
                    p.NewPushNote = true;
                    ctx.PushNotes.Add(NewNote);
                }
            }
            ctx.SaveChanges();
        }
        public ProfileModel GetCurrentUser(string Id)
        {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }
        public string GetNameOfLoggedInUser()
        {
            var ctx = new AppDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = GetCurrentUser(currentUserId);
            var Profile = ctx.Profiles.Find(currentUserId);
            var FirstName = Profile.FirstName;
            var LastName = Profile.LastName;

            return FirstName + " " + LastName;
        }

        
        public FileResult DownloadFile(int fileId)
        {
            var fileToDownload = _context.DevFiles.SingleOrDefault(f => f.FileId == fileId);

            byte[] fileBytes = fileToDownload.Content;
            string fileName = fileToDownload.Name;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        public ActionResult AddFile(DevelopmentViewModel model, HttpPostedFileBase upload)
        {
            var projectToUpdate = _context.Projects.First(p => p.Id == model.project.Id);

            if (projectToUpdate != null)
            {
                if (ModelState.IsValid)
                {
                    if (upload != null && upload.ContentLength > 0)
                    {
                        var file = new DevFile
                        {
                            Name = System.IO.Path.GetFileName(upload.FileName)

                        };
                        using (var reader = new System.IO.BinaryReader(upload.InputStream))
                        {
                            file.Content = reader.ReadBytes(upload.ContentLength);
                        }
                        projectToUpdate.Files.Add(file);
                        _context.SaveChanges();
                    }
                }
                
            }
            return RedirectToAction("EditDevelopmentPage", new { projectId = model.project.Id });
        }
    }
}