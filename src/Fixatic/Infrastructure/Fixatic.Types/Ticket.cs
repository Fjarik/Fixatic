using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.Types.Extensions;

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

		public TicketPriority Priority { get; set; }

		public TicketStatus Status { get; set; }

		public string Title { get; set; }

		public TicketType Type { get; set; }

		public TicketVisibility Visibility { get; set; }

		public string PriorityString => Priority.GetName() + " priority";
	}
}
