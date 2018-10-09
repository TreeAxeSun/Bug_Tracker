using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BugTracker.Models
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int FilePath { get; set; }
        public string Description { get; set; }
        public DateTimeOffset Created { get; set; }
        public int UserId { get; set; }
        public string FileUrl { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}