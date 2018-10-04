namespace BugTracker.Migrations
{
    using BugTracker.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugTracker.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(BugTracker.Models.ApplicationDbContext context)
        {

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Roles.Any(r => r.Name == "Admin"))
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
            }

            if (!context.Roles.Any(r => r.Name == "ProjectManager"))
            {
                roleManager.Create(new IdentityRole { Name = "ProjectManager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Users.Any(p => p.Email == "admin@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admi@tracker.com",
                    Email = "admin@tracker.com",
                    FirstName = "YS",
                    LastName = "Ahn",
                    DisplayName = "Master"
                }, "Adminuser@1");
            }

            if (!context.Users.Any(p => p.Email == "projectM@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectM@naver.com",
                    Email = "projectM@naver.com",
                    FirstName = "Sophia",
                    LastName = "Lee",
                    DisplayName = "Sophie"
                }, "ProjectM@1");
            }

            var adminId = userManager.FindByEmail("admin@tracker.com").Id;
            userManager.AddToRole(adminId, "Admin");


            var PMId = userManager.FindByEmail("projectM@tracker.com").Id;
            userManager.AddToRole(PMId, "ProjectManager");

        }
    }
}


//ApplicationUser adminUser;

//if (!context.Users.Any(r => r.UserName == "adminUser@tracker.com"))
//{
//    adminUser = new ApplicationUser();
//    adminUser.Email = "adminUser@tracker.com";
//    adminUser.UserName = "adminUser@tracker.com";

//    userManager.Create(adminUser, "Password-1");
//}
//else
//{
//    adminUser = context.Users.First(r => r.UserName == "adminUser@tracker.com");
//}

//if (!userManager.IsInRole(adminUser.Id, "Admin"))
//{
//    userManager.AddToRole(adminUser.Id, "Admin");
//}