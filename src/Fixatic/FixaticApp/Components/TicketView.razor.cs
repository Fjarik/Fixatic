using Fixatic.Types;
using Microsoft.AspNetCore.Components;

namespace FixaticApp.Components
{
	public partial class TicketView
	{
		[Parameter] public Ticket? Model { get; set; }

		[Parameter] public List<Comment>? Comments { get; set; }

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

		private async Task<string> LoadTestTicket()
		{
			var tickets = (await _ticketsService.GetAllAsync());
			
			if (tickets.IsSuccess && tickets.Item != null)
			{
				Model = tickets.Item[0];
			}

			if (!tickets.IsSuccess)
			{
				Console.WriteLine(tickets.Exception);
			}
			
            return "";
		}
	}
}