using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using ScrumProj.Models;

[assembly: OwinStartupAttribute(typeof(ScrumProj.Startup))]
namespace ScrumProj
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
        
        protected void InitRoles()
        {
            var context = new ApplicationDbContext();
            // User-identity context
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context));

            var roleManager = new RoleManager<IdentityRole>(
            new RoleStore<IdentityRole>(context));

            // Admin role:
            var roleName = "Admin";
            // Check to see if Role Exists, if not create it
            if (!roleManager.RoleExists(roleName))
            {
                roleManager.Create(new IdentityRole(roleName));
            }

            // Add user to role if not in role:
            var adminUser = userManager.FindByEmail("jocke@hotmail.com");

            if (!userManager.IsInRole(adminUser.Id, roleName))
            {
                userManager.AddToRole(adminUser.Id, roleName);
            }
        }
    }
}
