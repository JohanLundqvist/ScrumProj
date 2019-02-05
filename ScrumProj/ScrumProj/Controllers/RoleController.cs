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
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        ApplicationDbContext ctx = new ApplicationDbContext();

        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(
                new ApplicationDbContext()));

        // GET: Role
        public ActionResult Index()
        {
            var roles = ctx.Roles.ToList();
            return View(roles);
        }



        public ActionResult CreateRole()
        {
            return View();
        }

        // POST: /Roles/Create
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



        public ActionResult DeleteRole(string RoleName)
        {
            var thisRole = ctx.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            ctx.Roles.Remove(thisRole);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }



        //
        // GET: /Roles/Edit/5
        public ActionResult EditRole(string roleName)
        {
            var thisRole = ctx.Roles.Where(r => r.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            return View(thisRole);
        }

        //
        // POST: /Roles/Edit/5
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



        public ActionResult ManageRoles()
        {
            // prepopulat roles for the view dropdown
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr =>
            
            new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                ViewBag.RolesForThisUser = userManager.GetRoles(user.Id);

                // prepopulat roles for the view dropdown
                var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
                ViewBag.Roles = list;
            }

            return View("ManageRoles");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            //var account = new AccountController();
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            if (userManager.IsInRole(user.Id, RoleName))
            {
                userManager.RemoveFromRole(user.Id, RoleName);
                ViewBag.ResultMessage = "Role removed from this user successfully !";
            }
            else
            {
                ViewBag.ResultMessage = "This user doesn't belong to selected role.";
            }
            // prepopulat roles for the view dropdown
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            ApplicationUser user = ctx.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var idResult = userManager.AddToRole(user.Id, RoleName);

            //var account = new AccountController();
            //account.UserManager.AddToRole(user.Id, RoleName);

            ViewBag.ResultMessage = "Role created successfully !";

            // prepopulat roles for the view dropdown
            var list = ctx.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();
            ViewBag.Roles = list;

            return View("ManageRoles");
        }
    }
}