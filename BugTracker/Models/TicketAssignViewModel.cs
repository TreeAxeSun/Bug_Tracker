using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace BugTracker.Models
{

    public class TicketAssignViewModel 
    {
        public int Id { get; set; }
        public ICollection<ApplicationUser> Developers { get; set; }
        public SelectList DeveloperList { get; set; }
        public string[] SelectedDeveloper { get; set; }
        //public string Title { get; set; }
        //public int ProjectId { get; set; }

        public TicketAssignViewModel()
        {
            Developers = new HashSet<ApplicationUser>();
        }
    }
}