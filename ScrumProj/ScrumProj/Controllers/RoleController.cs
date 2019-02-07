﻿using Microsoft.AspNet.Identity;
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

        // Profile Database Context
        AppDbContext profileCtx = new AppDbContext();

        // UserManager connection
        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));



        // Method to view Profile requests or Roles
        public ActionResult Index()
        {
            var profiles = profileCtx.Profiles.Where(p => p.IsApproved.Equals(false));

            return View(profiles);
        }



        public ActionResult AcceptUser(string id)
        {
            var userProfile = profileCtx.Profiles.FirstOrDefault(p => p.ID == id);

            userProfile.IsApproved = true;

            profileCtx.SaveChanges();

            return RedirectToAction("Index");
        }



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

                ViewBag.Message = "Role created successfully!";

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
            }
            else
            {
                ViewBag.Message = "Var vänlig fyll i fältet!";
            }

            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

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

                    ViewBag.Message = "Rollen för den här användaren togs bort!";
                }
                else
                {
                    ViewBag.Message = "Den här användaren tillhör ingen roll!";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRoleToUser(string UserName, string RoleName)
        {
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var idResult = userManager.AddToRole(user.Id, RoleName);

            ViewBag.Message = "Role created successfully!";

            // Prepopulate the dropdown with roles
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }
    }
}
