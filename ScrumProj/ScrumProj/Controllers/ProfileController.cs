﻿using Microsoft.AspNet.Identity;
using ScrumProj.Models;
using ScrumProj.Models.ViewModels;
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
        // Index method to return a ProfileViewModel
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

            return View("ProfileCreated");
        }



        // Method to view Profile information
        public ActionResult ViewProfile()
        {
            var ctx = new AppDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUserProfile = ctx.Profiles.FirstOrDefault(p => p.ID == currentUserId);

            return View(new ProfileViewModel
            {
                FirstName = currentUserProfile.FirstName,
                LastName = currentUserProfile.LastName,
                Position = currentUserProfile.Position
            });
        }


        public ProfileModel GetCurrentUser(string Id) {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }

        public ActionResult PostFormalEntry(HttpPostedFileBase newFile)
        {
            AppDbContext db = new AppDbContext();
            if (newFile != null)
            {
                byte[] file;
                string fileName;
                using (var br = new BinaryReader(newFile.InputStream))
                {
                    file = br.ReadBytes((int)newFile.ContentLength);
                    fileName = Path.GetFileName(newFile.FileName);
                }
                db.Files.Add(new Models.File
                {
                    FileBytes = file,
                    FileName = fileName
                });
                db.SaveChanges();
                return RedirectToAction("BlogPage");
            }
            return RedirectToAction("BlogPage");
        }
        [Authorize]

        public ActionResult Booking(MeetingViewModel model)
        {
            var _context = new AppDbContext();
            var listOfUsers = new List<SelectListItem>();
            var listOfMeetings = new List<Meeting>();

            foreach(var m in _context.Meetings)
            {
                listOfMeetings.Add(m);
            }

            foreach(var user in _context.Profiles)
            {
                if (!user.ID.Equals(User.Identity.GetUserId()))
                {
                    var item = new SelectListItem
                    {
                        Text = user.FirstName + " " + user.LastName,
                        Value = user.ID,
                        Selected = false
                    };
                    listOfUsers.Add(item);
                }
            }

            var userId = User.Identity.GetUserId();
            var activeUser = _context.Profiles.First(u => u.ID == userId);

            model.UsersToAdd = listOfUsers;
            model.Meetings = listOfMeetings;
            model.User = activeUser;
            return View(model);
        }

        [Authorize]
        public ActionResult FirstPage(FirstPageViewModel model)
        {
            model.loggedInUser = GetCurrentUser(User.Identity.GetUserId());
            model.ListOfEntriesToLoopInBlogView = new List<EntryViewModel>();
            AppDbContext db = new AppDbContext();
            var ListofEntries = db.Entries.ToList();
            foreach (var entry in ListofEntries)
            {
                
                    var thisFileId = entry.fileId;
                    var FileToFetch = db.Files.SingleOrDefault(i => i.FileId == thisFileId);
                    model.ListOfEntriesToLoopInBlogView.Add(new EntryViewModel
                    {
                        entry = entry,
                        File = FileToFetch
                    });
            }
            model.ListOfComments = new List<Comment>();
            foreach (var c in db.Comments)
            {
                model.ListOfComments.Add(c);
            }
            model.Categories = new List<Categories>();
            foreach (var c in db.Categories)
            {
                model.Categories.Add(c);
            }

            model.CategoryIds = new List<CategoryInEntry>();
            foreach (var i in db.CategoryInEntrys)
            {
                model.CategoryIds.Add(i);
            }
                return View(model);
        }
        public ActionResult PostComment(EntryViewModel model, int postId)
        {
            var ctx = new AppDbContext();
            var currentUserId = User.Identity.GetUserId();
            var currentUser = GetCurrentUser(currentUserId);
            var comment = model.comment;
            ctx.Comments.Add(new Comment
            {
                comment = comment,
                EntryId = postId,
                Author = GetNameOfLoggedInUser()
            });
            ctx.SaveChanges();
            return RedirectToAction("FirstPage");
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
        public bool GetNewPushNoteStatus()
        {
            var ctx = new AppDbContext();
            var currentProfile = GetCurrentUser(User.Identity.GetUserId());

            return currentProfile.NewPushNote;
        }
        public ActionResult _PushNotes()
        {
            var model = new ProfileViewModel();
            model.NewPushNote = GetNewPushNoteStatus();
            return PartialView(model);
        }
        public ActionResult ReadPushNotes()
        {
            var ctx = new AppDbContext();
            var profile = ctx.Profiles.Find(User.Identity.GetUserId());
            profile.NewPushNote = false;
            var ListOfPushNotes = new List<PushNote>();
            string CurrentUserId = User.Identity.GetUserId();
            ListOfPushNotes = ctx.PushNotes.Where(n => n.ProfileModelId == CurrentUserId).ToList();
            ctx.PushNotes.RemoveRange(ListOfPushNotes);
            ctx.SaveChanges();
            return View(ListOfPushNotes);
        }
    }
}