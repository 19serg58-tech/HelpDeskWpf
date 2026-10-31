using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskWpf.Models
{
    public class Ticket
    { 
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime CreateDate{ get; set; }
        public int Priority { get; set; }
        public int StatusId { get; set; }
        public TicketStatus? Status { get; set; }// = new();
        public int AuthorId { get; set; }
        public User? Author { get; set; }// = new();
        public int? AssigneeId { get; set; }
        public User? Assignee { get; set; }
    }
}
