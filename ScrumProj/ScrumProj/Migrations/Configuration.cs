namespace ScrumProj.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using ScrumProj.Models;
    using System.Data.Entity.Migrations;

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
            string password = "hejhej123";

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
            user.UserName = "mathiashatakka@hotmail.com";
            user.Email = "mathiashatakka@hotmail.com";
            user.EmailConfirmed = true;
            var adminResult = UserManager.Create(user, password);

            // Add User Jocke to Role SuperAdmin
            if (adminResult.Succeeded)
            {
                var result = UserManager.AddToRole(user.Id, role1);
            }

            // Add a Profile to the SuperAdmin account
            string firstName = "Mathias";
            string lastName = "Hatakka";
            string position = "CEO";

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user.Id,
                FirstName = firstName,
                LastName = lastName,
                Position = position,
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user.Id
            });



            /*
             * ---------------------------------------------------------------------------------------
             * SIV's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user1 = new ApplicationUser();
            user1.UserName = "sivsvensson@hotmail.com";
            user1.Email = "sivsvensson@hotmail.com";
            user1.EmailConfirmed = true;

            UserManager.Create(user1, "hejhej123");

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user1.Id,
                FirstName = "Siv",
                LastName = "Svensson",
                Position = "Knegare",
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user1.Id
            });

            /*
             * ---------------------------------------------------------------------------------------
             * BERTIL's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user2 = new ApplicationUser();
            user2.UserName = "bertilragnarsson@hotmail.com";
            user2.Email = "bertilragnarsson@hotmail.com";
            user2.EmailConfirmed = true;

            UserManager.Create(user2, "hejhej123");

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user2.Id,
                FirstName = "Bertil",
                LastName = "Ragnarsson",
                Position = "Knegare",
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user2.Id
            });

            /*
             * ---------------------------------------------------------------------------------------
             * KARL-HENRIK's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user3 = new ApplicationUser();
            user3.UserName = "karlhenrikbulman@hotmail.com";
            user3.Email = "karlhenrikbulman@hotmail.com";
            user3.EmailConfirmed = true;

            UserManager.Create(user3, "hejhej123");

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user3.Id,
                FirstName = "Karl-Henrik",
                LastName = "Bulman",
                Position = "Städare",
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user3.Id
            });

            /*
             * ---------------------------------------------------------------------------------------
             * Jenny's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user4 = new ApplicationUser();
            user4.UserName = "jennyjansson@hotmail.com";
            user4.Email = "jennyjansson@hotmail.com";
            user4.EmailConfirmed = true;

            var jennyResult = UserManager.Create(user4, "hejhej123");

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user4.Id,
                FirstName = "Jenny",
                LastName = "Jansson",
                Position = "IT Admin",
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user4.Id
            });



            // Add User Jenny to Role Admin
            if (jennyResult.Succeeded)
            {
                UserManager.AddToRole(user4.Id, role2);
            }

            /*
             * ---------------------------------------------------------------------------------------
             * BENNY's PROFIL
             * ---------------------------------------------------------------------------------------
             */
            var user5 = new ApplicationUser();
            user5.UserName = "bennyfrisk@hotmail.com";
            user5.Email = "bennyfrisk@hotmail.com";
            user5.EmailConfirmed = true;

            var bennyResult = UserManager.Create(user5, "hejhej123");

            ctx.Profiles.AddOrUpdate(new ProfileModel
            {
                ID = user5.Id,
                FirstName = "Benny",
                LastName = "Frisk",
                Position = "Marknadsföringsansvarig",
                IsApproved = true
            });
            ctx.WantMailOrNoes.AddOrUpdate(new WantMailOrNo
            {
                BlogPost = true,
                Mail = true,
                Sms = true,
                UserId = user5.Id
            });



            // Add User Benny to Role Admin
            if (bennyResult.Succeeded)
            {
                UserManager.AddToRole(user5.Id, role2);
            }

            ctx.SaveChanges();
        }
    }
}
