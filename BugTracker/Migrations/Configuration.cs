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
                roleManager.Create(new IdentityRole { Name = "Project Manager" });
            }

            if (!context.Roles.Any(r => r.Name == "Developer"))
            {
                roleManager.Create(new IdentityRole { Name = "Developer" });
            }

            if (!context.Roles.Any(r => r.Name == "Submitter"))
            {
                roleManager.Create(new IdentityRole { Name = "Submitter" });
            }

            if (!context.Users.Any(p => p.Email == "adminUser@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "adminUser@bugtracker",
                    Email = "adminUser@bugtracker",
                    FirstName = "YS",
                    LastName = "Ahn",
                    DisplayName = "Master"
                }, "AdminUser@1");
            }

            if (!context.Users.Any(p => p.Email == "pmUser@bugtracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "pmUser@bugtracker.com",
                    Email = "pmUser@bugtracker.com",
                    FirstName = "YS",
                    LastName = "Ahn",
                    DisplayName = "Project Manager"
                }, "pmUser@1");
            }

            var adminId = userManager.FindByEmail("adminUser@bugtracker.com").Id;
            userManager.AddToRole(adminId, "Admin");

        }
    }
}
