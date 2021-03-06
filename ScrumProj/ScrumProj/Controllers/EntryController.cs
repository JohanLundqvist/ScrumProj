﻿using Microsoft.AspNet.Identity;
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
        public ActionResult PublishEntry(HttpPostedFileBase newFile, EntryViewModel model) {

            var ctx = new AppDbContext();
            model.loggedInUser = GetCurrentUser(User.Identity.GetUserId());
            var UserId = model.loggedInUser.ID;
            Models.File ThisFile = new Models.File();
            if (newFile != null)
            {
                ThisFile = SaveFileToDatabase(newFile);
                ctx.Files.Add(ThisFile);
                ctx.SaveChanges();
                int FileIdToUse = 1000000;
                //Loop to get the latest id from the file table.
                foreach (var f in ctx.Files)
                {
                    FileIdToUse = f.FileId;
                }
                ctx.Entries.Add(new Entry
                {
                    AuthorId = UserId,
                    Content = model.entry.Content,
                    Title = model.entry.Title,
                    fileId = FileIdToUse,
                    Author = GetNameOfLoggedInUser()
                });
                
            }
            else
            {
                ctx.Entries.Add(new Entry
                {
                    AuthorId = UserId,
                    Content = model.entry.Content,
                    Title = model.entry.Title,
                    Author = GetNameOfLoggedInUser()
                });
            }       
            ctx.SaveChanges();
            return RedirectToAction("BlogPage");
        }

        public ProfileModel GetCurrentUser(string Id)
        {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }

        [Authorize]
        public ActionResult BlogPage(EntryViewModel model)
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
            model.ListOfEntriesToLoopInBlogView.OrderByDescending(e => e.ListOfEntriesToLoopInBlogView);
            return View(model);
        }

        public Models.File SaveFileToDatabase(HttpPostedFileBase newFile)
        {
            AppDbContext db = new AppDbContext();
            
            byte[] file;
            string fileName;
            using (var br = new BinaryReader(newFile.InputStream))
            {
                file = br.ReadBytes((int)newFile.ContentLength);
                fileName = Path.GetFileName(newFile.FileName);
            }
            return (new Models.File
            {
                FileBytes = file,
                FileName = fileName
            });      
        }
        public FileResult DownLoadFile(int id)
        {
            AppDbContext db = new AppDbContext();

            var FileById = db.Files.Where(f => f.FileId == id).ToList().SingleOrDefault();

            byte[] fileBytes = FileById.FileBytes;
            string fileName = FileById.FileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

        }
        public ActionResult DeleteEntry(int postId) {
            var ctx = new AppDbContext();
            var entry = ctx.Entries.Find(postId);
            var fileID = entry.fileId;
            var file = ctx.Files.Find(fileID);
            if (file != null)
            {
                ctx.Files.Remove(file);
            }
            ctx.Entries.Remove(entry);
            ctx.SaveChanges();
            return RedirectToAction("BlogPage");
        }
        public ActionResult EditEntryView(EntryViewModel model, int postId, string Title, string Content, int? fileId)
        {
            var ctx = new AppDbContext();
            var ThisFile = new Models.File();           
            if (fileId != null)
            {
                var OldFile = ctx.Files.Find(fileId);
                var CurrentEntry = ctx.Entries.Find(postId);
                model.entry = new Entry();
                model.entry.Title = Title;
                model.entry.Content = Content;
                model.entry.Id = postId;
                model.entry.fileId = OldFile.FileId;                
            }
            else
            {
                var CurrentEntry = ctx.Entries.Find(postId);
                model.entry = new Entry();
                model.entry.Title = Title;
                model.entry.Content = Content;
                model.entry.Id = postId;
            }
            
            return View(model);
        }

        public ActionResult EditEntry(HttpPostedFileBase newFile, EntryViewModel model)
        {
            var ctx = new AppDbContext();
            if (newFile != null)
            {
                var ThisFile = new Models.File();
                ThisFile = SaveFileToDatabase(newFile);
                ctx.Files.Add(ThisFile);
                int FileIdToUse = 1000000;
                ctx.SaveChanges();
                foreach (var f in ctx.Files)
                {
                    FileIdToUse = f.FileId;
                }
                var post = ctx.Entries.Find(model.entry.Id);
                post.AuthorId = User.Identity.GetUserId();
                post.Content = model.entry.Content;
                post.Title = model.entry.Title;
                post.fileId = FileIdToUse;
                post.Author = GetNameOfLoggedInUser();
            }
            else if (model.entry.fileId != 0)
            {
                var post = ctx.Entries.Find(model.entry.Id);
                post.AuthorId = User.Identity.GetUserId();
                post.Content = model.entry.Content;
                post.Title = model.entry.Title;
                post.fileId = model.entry.fileId;
                post.Author = GetNameOfLoggedInUser();
            }
            else
            {
                var post = ctx.Entries.Find(model.entry.Id);
                post.AuthorId = User.Identity.GetUserId();
                post.Content = model.entry.Content;
                post.Title = model.entry.Title;
                post.Author = GetNameOfLoggedInUser();
            }
            ctx.SaveChanges();

            return RedirectToAction("BlogPage");
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
            return RedirectToAction("BlogPage");
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
        public ActionResult RemoveFile(int postId)
        {
            var ctx = new AppDbContext();
            var post = ctx.Entries.Find(postId);
            var i = post.fileId;
            var file = ctx.Files.Find(post.fileId);
            ctx.Files.Remove(file);
            post.fileId = 0;
            ctx.SaveChanges();
            return RedirectToAction("BlogPage");
        }

    }
}