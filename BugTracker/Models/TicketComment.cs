using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BugTracker.Models
{
    public class TicketComment
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ICollection<ApplicationUser> User { get; set; }
    }
}