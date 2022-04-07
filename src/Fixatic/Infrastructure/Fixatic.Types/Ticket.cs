using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class Ticket
    {
        public int TicketId { get; set; }

        public int ProjectId { get; set; }

        public int? AssignedUserId { get; set; }

        public int CreatorId { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        public DateTime? DateSolved { get; set; }

        public DateTime? Modified { get; set; }

        public int Priority { get; set; }

        public int Status { get; set; }

        public string Title { get; set; }

        public int Type { get; set; }

        public int Visibility { get; set; }
    }
}
