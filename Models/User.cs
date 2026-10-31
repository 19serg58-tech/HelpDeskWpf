using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelpDeskWpf.Models
{
    public class User
    { 
        public int Id { get; set; }
        public string Login { get; set; } = ""; 
        public string Password { get; set; } = "";
        public string Fullname { get; set; } = "";
        public int RoleId { get; set; }
        public Role? Role { get; set; }// = new();
        public List<Ticket>? CreatedTickets { get; set; }//=new();
        public List<Ticket>? AssignedTicket { get; set; }// = new();
    }
}
