namespace ScrumProj.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ScrumProj.Models;
    using System.Data.Entity.Migrations;
    // using Microsoft.AspNet.Identity;
    // using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<ScrumProj.Models.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ScrumProj.Models.AppDbContext context)
        {
            //  This method will be called after migrating to the latest version.
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            AppDbContext ctx = new AppDbContext();
            ApplicationDbContext identityCtx = new ApplicationDbContext();
            InitializeDb(identityCtx, ctx);
        }



        // Method to add default user and roles
        private void InitializeDb(ApplicationDbContext idCtx, AppDbContext ctx)
        {
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(idCtx));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(idCtx));

            string role1 = "SuperAdmin";
            string role2 = "Admin";
            string password = "anon69";

            string firstName = "Joakim";
            string lastName = "Asplund";
            string position = "CEO";
            

            // Create Role SuperAdmin if it does not exist
            if (!RoleManager.RoleExists(role1))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role1));
            }

            // Create Role Admin if it does not exist
            if (!RoleManager.RoleExists(role2))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role2));
            }

            // Create User = SuperAdmin with password
            var user = new ApplicationUser();
            user.UserName = "jocke@hotmail.com";
            user.Email = "jocke@hotmail.com";
            var adminResult = UserManager.Create(user, password);

            // Add User Admin to Role SuperAdmin
            if (adminResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, role1);
            }

            // Add a Profile
            ctx.Profiles.Add(new ProfileModel
            {
                ID = user.Id,
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                IsApproved = true
            });
        }
    }
}
