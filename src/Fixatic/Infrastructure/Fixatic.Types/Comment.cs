using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class Comment
	{
		public int CommentId { get; set; }

		public int TicketId { get; set; }

		public int UserId { get; set; }

		public string Content { get; set; }

		public DateTime Created { get; set; }

		public bool IsInternal { get; set; }
	}
}