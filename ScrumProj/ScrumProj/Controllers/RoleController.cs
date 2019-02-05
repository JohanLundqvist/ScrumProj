using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScrumProj.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScrumProj.Controllers
{
    public class RoleController : Controller
    {
        // Database connection
        ApplicationDbContext ctx = new ApplicationDbContext();

        // UserManager connection
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));



        // Method to view Profile requests or Roles
        public ActionResult Index()
        {
            var roles = ctx.Roles.ToList();
            return View(roles);
        }



        // Method to create a Role
        public ActionResult CreateRole()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult CreateRole(FormCollection collection)
        {
            try
            {
                ctx.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                ctx.SaveChanges();
                ViewBag.ResultMessage = "Role created successfully !";
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // Method to delete a Role
        public ActionResult DeleteRole(string RoleName)
        {
            var thisRole = ctx.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            ctx.Roles.Remove(thisRole);
            ctx.SaveChanges();

            return RedirectToAction("Index");
        }



        // Method to edit a Role name
        public ActionResult EditRole(string roleName)
        {
            var thisRole = ctx.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole(Microsoft.AspNet.Identity.EntityFramework.IdentityRole role)
        {
            try
            {
                ctx.Entry(role).State = System.Data.Entity.EntityState.Modified;
                ctx.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }



        // Method to prepopulate fields (ManageRoles first page)
        public ActionResult ManageRoles()
        {
            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View();
        }



        // Method to list Roles for a user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // Prepopulate the dropdown with roles
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageRoles");
        }



        // Method to delete a Role for a User
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

                    ViewBag.Message = "Role removed from this user successfully!";
                }
                else
                {
                    ViewBag.Message = "This user does not belong to selected role!";
                }

                // Prepopulate the dropdown with roles
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }
            catch
            {
                ViewBag.Message = "Please fill in all the fields!";

                // Prepopulate the dropdown with roles
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
                new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageRoles");
        }

        

        // Method to add a Role to a User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var idResult = userManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }
    }
}
