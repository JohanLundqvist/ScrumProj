using Microsoft.AspNet.Identity;
using ScrumProj.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class EntryController : Controller
    {
        // Method
        [Authorize]
        public ActionResult PublishEntry(HttpPostedFileBase newFile, EntryViewModel model, string SelectBlogg, string tags, HttpPostedFileBase img)
        {
            var ctx = new AppDbContext();            
            model.loggedInUser = GetCurrentUser(User.Identity.GetUserId());
            var UserId = model.loggedInUser.ID;
            bool IsFormal = false;
            if (SelectBlogg == "1")
            IsFormal = true;
            Models.File ThisFile = new Models.File();

            // Adds data to entry with file
            if (newFile != null)
            {
                ThisFile = SaveFileToDatabase(newFile);
                ctx.Files.Add(ThisFile);
                ctx.SaveChanges();
                int FileIdToUse = 1000000;
                var imageUrl = "";

                // Loop to get the latest id from the file table.
                foreach (var f in ctx.Files)
                {
                    FileIdToUse = f.FileId;
                }

                if (img != null && img.ContentLength > 0)
                {
                    string imgName = Path.GetFileName(img.FileName);
                    string imgExtension = Path.GetExtension(imgName);

                    // Checks if the image file is an actual image
                    if (imgName.EndsWith("JPG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("JPEG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("PNG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("GIF", StringComparison.OrdinalIgnoreCase))
                    {
                        // Renames the image and saves it in folder
                        string newImgName = DateTime.Now.ToString("ddMMyyhhmmss") + imgExtension;
                        string url = Path.Combine(Server.MapPath("~/Images/EntryImg"), newImgName);
                        img.SaveAs(url);

                        imageUrl = "/Images/EntryImg/" + newImgName;

                        ctx.Entries.Add(new Entry
                        {
                            AuthorId = UserId,
                            Content = model.entry.Content,
                            Title = model.entry.Title,
                            fileId = FileIdToUse,
                            Author = GetNameOfLoggedInUser(),
                            Formal = IsFormal,
                            ImageUrl = imageUrl
                        });
                    }
                    else
                    {
                        ViewBag.Message = "Bilden får bara ha något av följande format: .jpg .jpeg .png eller .gif!";
                    }
                }
                else
                {
                    ctx.Entries.Add(new Entry
                    {
                        AuthorId = UserId,
                        Content = model.entry.Content,
                        Title = model.entry.Title,
                        fileId = FileIdToUse,
                        Author = GetNameOfLoggedInUser(),
                        Formal = IsFormal,
                        ImageUrl = imageUrl
                    });
                }
            }
            
            // Adds data to file without file
            else
            {
                var imageUrl = "";

                if (img != null && img.ContentLength > 0)
                {
                    string imgName = Path.GetFileName(img.FileName);
                    string imgExtension = Path.GetExtension(imgName);

                    // Checks if the image file is an actual image
                    if (imgName.EndsWith("JPG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("JPEG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("PNG", StringComparison.OrdinalIgnoreCase) ||
                        imgName.EndsWith("GIF", StringComparison.OrdinalIgnoreCase))
                    {
                        // Renames the image and saves it in folder
                        string newImgName = DateTime.Now.ToString("ddMMyyhhmmss") + imgExtension;
                        string url = Path.Combine(Server.MapPath("~/Images/EntryImg"), newImgName);
                        img.SaveAs(url);
                        
                        imageUrl = "/Images/EntryImg/" + newImgName;

                        ctx.Entries.Add(new Entry
                        {
                            AuthorId = UserId,
                            Content = model.entry.Content,
                            Title = model.entry.Title,
                            Author = GetNameOfLoggedInUser(),
                            Formal = IsFormal,
                            ImageUrl = imageUrl
                        });
                    }
                    else
                    {
                        ViewBag.Message = "Bilden får bara ha formeten: .jpg .jpeg .png eller .gif!";
                    }
                }
                else
                {
                    ctx.Entries.Add(new Entry
                    {
                        AuthorId = UserId,
                        Content = model.entry.Content,
                        Title = model.entry.Title,
                        Author = GetNameOfLoggedInUser(),
                        Formal = IsFormal,
                        ImageUrl = imageUrl
                    });
                }
            }
            ctx.SaveChanges();
            
            int postId = 100000000;

            // Gets the entry just saved to database
            foreach (var f in ctx.Entries)
            {
                postId = f.Id;
            }
            AddCategoryToDatabase(tags, postId);
            ctx.SaveChanges();

            NewPushNote(GetNameOfLoggedInUser() + " Har skrivit ett inlägg med titeln: " + model.entry.Title + "-" + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt"), "bloggPost");
            var ap = new ApplicationDbContext();
            List<string> Emails = new List<string>();
            foreach (var p in ap.Users)
            {
                var b = ctx.WantMailOrNoes.Where(u => u.UserId == p.Id).Single().Mail;
                if (b)
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
                Message = s + " Har skrivit ett inlägg med titeln: " + model.entry.Title + "-" + DateTime.Now.ToString(@"MM\/dd\/yyyy h\:mm tt")
            }, Emails));

            return RedirectToAction("BlogPage"); 
        }



        // Method
        public ProfileModel GetCurrentUser(string Id)
        {
            var ctx = new AppDbContext();
            var UserId = User.Identity.GetUserId();
            var appUser = ctx.Profiles.SingleOrDefault(u => u.ID == Id);

            return appUser;
        }



        // Method
        [Authorize]
        public ActionResult BlogPage(EntryViewModel model)
        {
            model.loggedInUser = GetCurrentUser(User.Identity.GetUserId());
            model.ListOfEntriesToLoopInBlogView = new List<EntryViewModel>();
            model.ListOfInformalEntriesToLoopInBlogView = new List<EntryViewModel>();

            AppDbContext db = new AppDbContext();
            var ListofEntries = db.Entries.ToList();

            foreach (var entry in ListofEntries)
            {
                if (entry.Formal == true)
                {
                    var thisFileId = entry.fileId;
                    var FileToFetch = db.Files.SingleOrDefault(i => i.FileId == thisFileId);

                    model.ListOfEntriesToLoopInBlogView.Add(new EntryViewModel
                    {
                        entry = entry,
                        File = FileToFetch
                    });
                }
                else
                {
                    var thisFileId = entry.fileId;
                    var FileToFetch = db.Files.SingleOrDefault(i => i.FileId == thisFileId);

                    model.ListOfInformalEntriesToLoopInBlogView.Add(new EntryViewModel
                    {
                        entry = entry,
                        File = FileToFetch
                    });

                }
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



        // Method
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



        // Method
        public FileResult DownLoadFile(int id)
        {
            AppDbContext db = new AppDbContext();

            var FileById = db.Files.Where(f => f.FileId == id).ToList().SingleOrDefault();

            byte[] fileBytes = FileById.FileBytes;
            string fileName = FileById.FileName;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);   
        }



        // Method
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



        // Method
        public ActionResult EditEntryView(EntryViewModel model, int postId, string Title, string Content, int? fileId)
        {
            var ctx = new AppDbContext();
            var ThisFile = new Models.File();

            if (model == null)
            {
                model = new EntryViewModel();
            }

            if (fileId != null && fileId != 0)
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

            model.Categories = new List<Categories>();
            foreach (var c in ctx.Categories)
            {
                model.Categories.Add(c);
            }

            model.CategoryIds = new List<CategoryInEntry>();
            foreach (var i in ctx.CategoryInEntrys)
            {
                model.CategoryIds.Add(i);
            }

            return View(model);
        }



        // Method
        public ActionResult EditEntry(HttpPostedFileBase newFile, EntryViewModel model, string tags)
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
            AddCategoryToDatabase(tags, model.entry.Id);
            ctx.SaveChanges();

            return RedirectToAction("BlogPage");
        }



        // Method
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



        // Method
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



        // Method
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



        // Method
        public JsonResult GetSearchValue(string search)
        {
            var ctx = new AppDbContext();
            List<string> allsearch = ctx.Categories.Where(x => x.Name.Contains(search)).Select(x => x.Name).ToList();

            return new JsonResult { Data = allsearch, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        // Method
        public ActionResult TestView()
        {
            var ctx = new AppDbContext();
            var model = ctx.Categories.ToList();

            return View(model);
        }



        // Method
        public void AddCategoryToDatabase(string searchInput, int latestPost)
        {
            var ctx = new AppDbContext();
            // splits categories from the input with space and puts them in array
            Regex rgx = new Regex("[^a-zA-Z ]");

            var fixedString = rgx.Replace(searchInput, "");
            var strArray = fixedString.ToLower().Split(' ');

            foreach (var str in strArray)
            {
                if (str != "")
                {
                    string strWithHash = "#" + str;

                    //Checks if ínputdata matches with categories in the database.
                    if (ctx.Categories.Any(x => x.Name == str))
                    {
                        ctx.CategoryInEntrys.Add(new CategoryInEntry
                        {
                            EntryId = latestPost,
                            CategoryId = ctx.Categories.Where(s => s.Name == str).Select(i => i.Id).Single()
                        });
                    }

                    //Checks if ínputdata matches with categories in the database if we add the # in the input.
                    else if (ctx.Categories.Any(x => x.Name == strWithHash))
                    {
                        ctx.CategoryInEntrys.Add(new CategoryInEntry
                        {
                            EntryId = latestPost,
                            CategoryId = ctx.Categories.Where(s => s.Name == strWithHash).Select(i => i.Id).Single()
                        });
                    }

                    //If the input doesn't exist in the database we add them.
                    else if (!ctx.Categories.Any(x => x.Name == str))
                    {
                        if (str.StartsWith("#"))
                        {
                            ctx.Categories.Add(new Categories
                            {
                                Name = str
                            });
                        }
                        else
                        {
                            ctx.Categories.Add(new Categories
                            {
                                Name = "#" + str
                            });
                        }
                        ctx.SaveChanges();

                        int CatId = 100000000;
                        foreach (var f in ctx.Categories)
                        {
                            CatId = f.Id;
                        }

                        ctx.CategoryInEntrys.Add(new CategoryInEntry
                        {
                            EntryId = latestPost,
                            CategoryId = CatId
                        });
                        ctx.SaveChanges();
                    }
                }
                ctx.SaveChanges();
            }
        }



        // Method
        public ActionResult DeleteCategory(int catId, int postId)
        {
            var ctx = new AppDbContext();
            var categoryToDelete = ctx.CategoryInEntrys.Find(catId);

            ctx.CategoryInEntrys.Remove(categoryToDelete);
            ctx.SaveChanges();

            var entry = ctx.Entries.Find(postId);
            var model = new EntryViewModel();

            return RedirectToAction("EditEntryView", new
            {
                model,
                postId,
                entry.Title,
                entry.Content,
                entry.fileId
            });
        }

        // Method
        public ActionResult _DeleteCategoryPartial(EntryViewModel model)
        {
            return PartialView(model);
        }

        public void NewPushNote(string note, string typeOfNote)
        {
            var ctx = new AppDbContext();

            foreach (var p in ctx.Profiles)
            {
                var b = ctx.WantMailOrNoes.Where(s => s.UserId == p.ID).Single().BlogPost;
                if (b)
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
        public void NewPushNote(string note)
        {
            var ctx = new AppDbContext();

            foreach (var p in ctx.Profiles)
            {
                var NewNote = new PushNote
                {
                    Note = note,
                    ProfileModelId = p.ID
                };
                p.NewPushNote = true;
                ctx.PushNotes.Add(NewNote);
            }
            ctx.SaveChanges();
        }
    }
}