using System;
using System.Data.Entity;
using System.Linq;

namespace BugTracker.Models
{
    public class Dashboard : DbContext
    {
        public int AllUsers { get; set; }
        public int AllProjects { get; set; }
        public int AllTickets { get; set; }
    }
}