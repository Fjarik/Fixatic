using Fixatic.Types;
using Microsoft.AspNetCore.Components;

namespace FixaticApp.Components
{
	public partial class TicketView
	{
		[Parameter] public Ticket? Model { get; set; }

		[Parameter] public List<Comment>? Comments { get; set; }

		[Parameter] public List<string> commentsString { get; set; } = new List<string>()
		{
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
			"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. ",
		};
		
		[Parameter] public Attachement? Attachement { get; set; }

		[Parameter] public User? Assignee { get; set; }

		private string GetAssigneeName()
		{
			if (Assignee == null)
			{
				return "None";
			}
			else
			{
				return Assignee.Lastname + Assignee.Firstname;
			}
		}
	}
}