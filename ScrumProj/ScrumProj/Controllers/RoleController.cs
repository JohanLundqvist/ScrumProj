using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrumProj.Models;
using ScrumProj.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    [Authorize(Roles = "Admin, SuperAdmin")]
    public class RoleController : Controller
    {
        // Database connection
        ApplicationDbContext ctx = new ApplicationDbContext();

        // Profile Database Context
        AppDbContext profileCtx = new AppDbContext();

        // UserManager connection
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));



        // Method to view Profile requests
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult Index()
        {
            var vm = new ProfileViewModel();
            var profiles = profileCtx.Profiles.Where(p => p.IsApproved.Equals(false)).ToList();
            vm.Categories = new List<Categories>();

            foreach (var c in profileCtx.Categories)
            {
                vm.Categories.Add(c);
            }
            vm.Profiles = profiles;

            return View(vm);
        }



        // Method to accept a user
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult AcceptUser(string id)
        {
            var userProfile = profileCtx.Profiles.FirstOrDefault(p => p.ID == id);

            userProfile.IsApproved = true;

            profileCtx.SaveChanges();

            return RedirectToAction("Index");
        }



        // Method to completely remove a user
        [Authorize(Roles = "Admin, SuperAdmin")]
        public ActionResult RemoveUser(string id)
        {
            var userProfile = profileCtx.Profiles.FirstOrDefault(p => p.ID == id);
            var userIdentity = ctx.Users.FirstOrDefault(p => p.Id == id);

            profileCtx.Profiles.Remove(userProfile);
            ctx.Users.Remove(userIdentity);

            profileCtx.SaveChanges();
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }



        // Method to prepopulate fields (ManageRoles first page)
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ManageRoles()
        {
            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View();
        }


        
        // Method to list Roles for a user
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            var profile = ctx.Users.FirstOrDefault(p => p.UserName == UserName);

            if (!string.IsNullOrWhiteSpace(UserName) && profile != null)
            {
                ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);
            }
            else
            {
                ViewBag.Message = "Var vänlig fyll i fältet och ange i en giltig E-post adress!";
            }

            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }



        // Method to delete a Role for a User
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName, string name)
        {
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            try
            {
                if (userManager.IsInRole(user.Id, RoleName))
                {
                    userManager.RemoveFromRole(user.Id, RoleName);

                    ViewBag.Message = "Rollen för den här användaren togs bort!";
                }
                else
                {
                    ViewBag.Message = "Den här användaren tillhör inte den här rollen!";
                }

                // Prepopulate the dropdown with roles
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }
            catch
            {
                ViewBag.Message = "Var vänlig fyll i alla fält och ange en korrekt E-mail!";

                // Prepopulate the dropdown with roles
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageRoles");
        }



        // Method to add a Role to a User
        [Authorize(Roles = "SuperAdmin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var idResult = userManager.AddToRole(user.Id, RoleName);

            ViewBag.Message = "Det lyckades!";

            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }



        // Method to return all users who are in the Role "Admin"
        [Authorize(Roles = "SuperAdmin")]
        public ActionResult ListAllAdmins()
        {
             AdminListViewModel listOfAdmins = new AdminListViewModel();
            
            var roleId = ctx.Roles.FirstOrDefault(r => r.Name == "Admin").Id;
            var Users = ctx.Users.ToList();

            foreach (var user in Users)
            {
                var users = user.Roles.FirstOrDefault(u => u.RoleId == roleId);

                if (users != null)
                {
                    listOfAdmins.AdminList.Add(user);
                }
            }

            return PartialView(listOfAdmins);
        }


        // Method
        public ActionResult DeleteCategory(int catId)
        {
            var ctx = new AppDbContext();
            var Category = ctx.Categories.Find(catId);
            ctx.Categories.Remove(Category);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
