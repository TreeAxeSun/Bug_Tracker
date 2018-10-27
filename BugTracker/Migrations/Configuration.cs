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
                    FirstName = "Admin",
                    LastName = "Admin",
                    FullName = "Admin"
                }, "Adminuser@1");
            }

            if (!context.Users.Any(p => p.Email == "demoadmin@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demoadmin@tracker.com",
                    Email = "demoadmin@tracker.com",
                    FirstName = "DemoAdmin",
                    LastName = "DemoAdmin",
                    FullName = "DemoAdmin"
                }, "1234qweR!");
            }

            if (!context.Users.Any(p => p.Email == "demodev@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demodev@tracker.com",
                    Email = "demodev@tracker.com",
                    FirstName = "DemoDeveloper",
                    LastName = "DemoDeveloper",
                    FullName = "DemoDeveloper"
                }, "1234qweR!");
            }

            if (!context.Users.Any(p => p.Email == "demopro@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demopro@tracker.com",
                    Email = "demopro@tracker.com",
                    FirstName = "DemoProjectManager",
                    LastName = "DemoProjectManager",
                    FullName = "DemoProjectManager"
                }, "1234qweR!");
            }

            if (!context.Users.Any(p => p.Email == "demosub@tracker.com"))
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demosub@tracker.com",
                    Email = "demosub@tracker.com",
                    FirstName = "DemoSubmitter",
                    LastName = "DemoSubmitter",
                    FullName = "DemoSubmitter"
                }, "1234qweR!");
            }

            string adminId = userManager.FindByEmail("admin@tracker.com").Id;
            userManager.AddToRole(adminId, "Admin");

            string demoAdminId = userManager.FindByEmail("demoadmin@tracker.com").Id;
            userManager.AddToRole(demoAdminId, "Admin");

            string demodevId = userManager.FindByEmail("demodev@tracker.com").Id;
            userManager.AddToRole(demodevId, "Developer");

            string demoproId = userManager.FindByEmail("demopro@tracker.com").Id;
            userManager.AddToRole(demoproId, "ProjectManager");

            string demosubId = userManager.FindByEmail("demosub@tracker.com").Id;
            userManager.AddToRole(demosubId, "Submitter");


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
