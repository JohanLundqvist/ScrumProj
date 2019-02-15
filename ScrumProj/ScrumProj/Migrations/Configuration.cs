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
            /*
             * ---------------------------------------------------------------------------------------
             * Adds a SuperAdmin account and Profile
             * ---------------------------------------------------------------------------------------
             */
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(idCtx));
            var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(idCtx));

            string role1 = "SuperAdmin";
            string role2 = "Admin";
            string password = "anon69";

            // Create SuperAdmin role if it does not exist
            if (!RoleManager.RoleExists(role1))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role1));
            }

            // Creates Admin role if it does not exist
            if (!RoleManager.RoleExists(role2))
            {
                var roleresult = RoleManager.Create(new IdentityRole(role2));
            }

            // Create User = SuperAdmin with password
            var user = new ApplicationUser();
            user.UserName = "jocke@hotmail.com";
            user.Email = "jocke@hotmail.com";
            user.EmailConfirmed = true;
            var adminResult = UserManager.Create(user, password);

            // Add User Admin to Role SuperAdmin
            if (adminResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, role1);
            }

            // Add a Profile to the SuperAdmin account
            string firstName = "Joakim";
            string lastName = "Asplund";
            string position = "CEO";

            ctx.Profiles.Add(new ProfileModel
            {
                ID = user.Id,
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                IsApproved = true
            });



            /*
             * ---------------------------------------------------------------------------------------
             * DAVID's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user1 = new ApplicationUser();
            user1.UserName = "davidlingvall@gmail.com";
            user1.Email = "davidlingvall@gmail.com";
            user1.EmailConfirmed = true;

            UserManager.Create(user1, "hej12345");

            ctx.Profiles.Add(new ProfileModel
            {
                ID = user1.Id,
                FirstName = "David",
                LastName = "Lindkuk",
                Position = "Elektriker",
                IsApproved = true
            });

            /*
             * ---------------------------------------------------------------------------------------
             * SVING's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user2 = new ApplicationUser();
            user2.UserName = "sving91@gmail.com";
            user2.Email = "sving91@gmail.com";
            user2.EmailConfirmed = true;

            UserManager.Create(user2, "hej12345");

            ctx.Profiles.Add(new ProfileModel
            {
                ID = user2.Id,
                FirstName = "Svinger",
                LastName = "Lillkuk",
                Position = "Rövslickare",
                IsApproved = true
            });

            /*
             * ---------------------------------------------------------------------------------------
             * TEST's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user3 = new ApplicationUser();
            user3.UserName = "test@hotmail.com";
            user3.Email = "test@mail.com";
            user3.EmailConfirmed = true;

            UserManager.Create(user3, "12344321");

            ctx.Profiles.Add(new ProfileModel
            {
                ID = user3.Id,
                FirstName = "Hugh",
                LastName = "Mungus",
                Position = "Fattaru??",
                IsApproved = true
            });

            ctx.SaveChanges();
        }
    }
}
