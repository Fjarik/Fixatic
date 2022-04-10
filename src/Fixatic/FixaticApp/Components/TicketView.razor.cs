using Fixatic.Types;
using Microsoft.AspNetCore.Components;

namespace FixaticApp.Components
{
	public partial class TicketView
	{
		[Parameter] public Ticket? Model { get; set; }

		public List<Comment>? Comments { get; set; }

		public Attachement? Attachement { get; set; }

		public User? Assignee { get; set; }

		private int PrevId { get; set; } = -1;

		private string GetAssigneeName()
		{
			if (Assignee == null)
			{
				return "None";
			}

			return Assignee.GetFullName();
		}

		protected override async Task OnParametersSetAsync()
		{
			if (Model == null)
				return;

			if (PrevId == Model.TicketId)
				return;

			PrevId = Model.TicketId;

			await Task.CompletedTask;
			// await LoadTestTicket();
		}

		private async Task LoadTestTicket()
		{
			var tickets = (await _ticketsService.GetAllAsync());

			if (!tickets.IsSuccess)
			{
				return;
			}

			Model = tickets.Item[0];
		}
	}
}