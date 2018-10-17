using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace BugTracker.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ICollection<Project> AssignedProjects(string userId)
        {
            ApplicationUser user = db.Users.Find(userId);
            var projects = user.Projects.ToList();
            return (projects);
        }

        [Authorize(Roles = "ProjectManager,Developer")]
        public ActionResult AssignedProjectTicket()
        {
            var OwnTickets = new List<Ticket>();
            var userId = User.Identity.GetUserId();
            var OwnProjects = AssignedProjects(userId);
            foreach (var project in OwnProjects)
            {
                OwnTickets.AddRange(db.Tickets.Where(t => t.ProjectId == project.Id));
            }
            return View("AssignedProjectTicket", OwnTickets.ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult AdminTicket()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser)
                                    .Include(t => t.OwnerUser)
                                    .Include(t => t.TicketPriority)
                                    .Include(t => t.TicketStatus)
                                    .Include(t => t.TicketType);
            return View("AdminTicket", tickets.ToList());
        }

        [Authorize(Roles = "Submitter")]
        public ActionResult SubmitterTicket()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.OwnerUserId == userId)
                                                .Include(t => t.AssignedToUser)
                                                .Include(t => t.OwnerUser)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType);
            return View("SubmitterTicket", tickets.ToList());
        }

        [Authorize(Roles = "Developer")]
        public ActionResult DeveloperTicket()
        {
            var userId = User.Identity.GetUserId();
            var tickets = db.Tickets.Where(t => t.AssignedToUserId == userId)
                                                .Include(t => t.AssignedToUser)
                                                .Include(t => t.OwnerUser)
                                                .Include(t => t.TicketPriority)
                                                .Include(t => t.TicketStatus)
                                                .Include(t => t.TicketType);
            return View("DeveloperTicket", tickets.ToList());
        }


        // GET: Tickets
        public ActionResult Index()
        {
            var tickets = db.Tickets.Include(t => t.AssignedToUser)
                                    .Include(t => t.OwnerUser)
                                    .Include(t => t.TicketPriority)
                                    .Include(t => t.TicketStatus)
                                    .Include(t => t.TicketType);
            return View(db.Tickets.ToList());
        }


        // GET: Tickets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }

            return View(ticket);
        }




        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullName");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name");
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");

            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Submitter")]
        public ActionResult Create([Bind(Include = "Id,Title,Description,ProjectId,AssignedToUserId,TicketPriorityId,TicketStatusId,TicketTypeId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusId = 5;
                ticket.OwnerUserId = User.Identity.GetUserId();
                ticket.Created = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FullName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FullName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);

            if (ticket == null)
            {
                return HttpNotFound();
            }

            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Created,ProjectId,TicketTypeId,TicketPriorityId,TicketStatusId,OwnerUserId,AssignedToUserId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                Ticket ticketDb = db.Tickets.Where(t => t.Id == ticket.Id).FirstOrDefault();
                ticketDb.Title = ticket.Title;
                ticketDb.Description = ticket.Description;
                //ticketDb.ProjectId = ticket.ProjectId;
                ticketDb.TicketTypeId = ticket.TicketTypeId;
                ticketDb.TicketPriorityId = ticket.TicketPriorityId;
                ticketDb.TicketStatusId = ticket.TicketStatusId;
                ticketDb.Updated = DateTimeOffset.Now;
                ticket.OwnerUserId = User.Identity.GetUserId();
                //ticketDb.AssignedToUserId = User.Identity.GetUserId();
                //db.Tickets.Add(ticket);
                //db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Tickets");
            }
            ViewBag.AssignedToUserId = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserId);
            ViewBag.OwnerUserId = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name", ticket.ProjectId);
            ViewBag.TicketPriorityId = new SelectList(db.TicketPriorities, "Id", "Name", ticket.TicketPriorityId);
            ViewBag.TicketStatusId = new SelectList(db.TicketStatus, "Id", "Name", ticket.TicketStatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

        public ICollection<ApplicationUser> RoleManager(string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = UserManager.Users.ToList();
            foreach(var user in List)
            {
                if(IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            return UserManager.IsInRole(userId, roleName);
        }

        //GET
        [Authorize(Roles = "ProjectManager")]
        public ActionResult AssignTickets(int id)
        {
            var model = new TicketAssignViewModel();
            model.Id = id;
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == id);
            model.Developers = RoleManager("Developer");

            var userIdsAssignedToTicket = ticket.Users.Select(t => t.Id).ToList();

            model.DeveloperList = new SelectList(model.Developers, "Id", "FullName", userIdsAssignedToTicket);

            return View(model);
        }

        //POST
        [Authorize(Roles = "ProjectManager")]
        [HttpPost]
        public ActionResult AssignTickets(TicketAssignViewModel model)
        {
            //STEP 1: Find the Ticket
            var ticket = db.Tickets.FirstOrDefault(t => t.Id == model.Id);

            //STEP 2: Remove all assigned users from this ticket
            var assignedUsers = ticket.Users.ToList();

            foreach (var user in assignedUsers)
            {
                ticket.Users.Remove(user);
            }

            //STEP 3: Assign users to the ticket
            if (model.SelectedDeveloper != null)
            {
                foreach (var userId in model.SelectedDeveloper)
                {
                    var user = db.Users.FirstOrDefault(t => t.Id == userId);

                    ticket.Users.Add(user);
                    ticket.AssignedToUserId = user.Id; 
                }
            }

            //STEP 4: Save changes to the database
            
            db.SaveChanges();

            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
