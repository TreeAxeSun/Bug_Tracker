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

        protected override void Seed(ApplicationDbContext context)
        {
            RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            
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

            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!context.Users.Any(p => p.Email == "admin@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "admin@tracker.com",
                    Email = "admin@tracker.com",
                    FirstName = "YS",
                    LastName = "Ahn",
                    FullName = "YS Ahn"
                }, "Adminuser@1");
            }

            if (!context.Users.Any(p => p.Email == "projectM@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "projectM@tracker.com",
                    Email = "projectM@tracker.com",
                    FirstName = "Sophie",
                    LastName = "Lee",
                    FullName = "Sophie Lee"
                }, "ProjectM@1");
            }

            if (!context.Users.Any(p => p.Email == "developer@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "developer@tracker.com",
                    Email = "developer@tracker.com",
                    FirstName = "Delphie",
                    LastName = "Lee",
                    FullName = "Delphie Lee"
                }, "Developer@1");
            }

            string adminId = userManager.FindByEmail("admin@tracker.com").Id;
            userManager.AddToRole(adminId, "Admin");

            string PMId = userManager.FindByEmail("projectM@tracker.com").Id;
            userManager.AddToRole(PMId, "ProjectManager");

            string developerId = userManager.FindByEmail("developer@tracker.com").Id;
            userManager.AddToRole(developerId, "Developer");


            context.TicketPriorities.AddOrUpdate(p=>p.Name,
               new TicketPriority { Name = "High" },
               new TicketPriority { Name = "Medium" },
               new TicketPriority { Name = "Low" },
               new TicketPriority { Name = "Urgent" }
            );
            context.TicketTypes.AddOrUpdate(t=>t.Name,
               new TicketType { Name = "Bug Fixes" },
               new TicketType { Name = "Software Update" },
               new TicketType { Name = "Database Error" }
           );
            context.TicketStatus.AddOrUpdate(s=>s.Name,
              new TicketStatus { Name = "Not Started" },
              new TicketStatus { Name = "Finished" },
              new TicketStatus { Name = "On Hold" },
              new TicketStatus { Name = "In Progress" }
           );
        }
    }
}
