using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
                foreach (var f in ctx.Files)
                {
                    FileIdToUse = f.FileId;
                }
                ctx.Entries.Add(new Entry
                {
                    AuthorId = UserId,
                    Content = model.entry.Content,
                    Title = model.entry.Title,
                    fileId = FileIdToUse
                });
                
            }
            else
            {
                ctx.Entries.Add(new Entry
                {
                    AuthorId = UserId,
                    Content = model.entry.Content,
                    Title = model.entry.Title
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

        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                string pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(
                                       Server.MapPath("~/images/profile"), pic);
                // file is uploaded
                file.SaveAs(path);

                // save the image path path to the database or you can send image
                // directly to database
                // in-case if you want to store byte[] ie. for DB
                using (MemoryStream ms = new MemoryStream())
                {
                    file.InputStream.CopyTo(ms);
                    byte[] array = ms.GetBuffer();
                }

            }
            // after successfully uploading redirect the user
            return RedirectToAction("actionname", "controller name");
        }

        public FileContentResult EntryImages()
        {
            if (User.Identity.IsAuthenticated)
            {
                String entryId = User.Identity.GetUserId();

                if (entryId == null)
                {
                    String fileName = HttpContext.Server.MapPath(@"~/EntryImages/noImg.png");

                    byte[] imageData = null;
                    FileInfo fileInfo = new FileInfo(fileName);
                    long imageFileLength = fileInfo.Length;
                    FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fs);
                    imageData = br.ReadBytes((int)imageFileLength);

                    return File(imageData, "image/png");
                }

                var bdEntry = HttpContext.GetOwinContext().Get<ApplicationDbContext>();
                var entryImage = bdEntry.Users.Where(x => x.Id == entryId).FirstOrDefault();

                return new FileContentResult(entryImage.entryImage, "image/jpeg");
            } else
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");

                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }
        }



    }
}