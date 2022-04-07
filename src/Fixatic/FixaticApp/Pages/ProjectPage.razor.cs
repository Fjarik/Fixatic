using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Pages;

public partial class ProjectPage
{
	[Parameter] public int ProjectId { get; set; }

	[Parameter] public Project? Project { get; set; }


	private MudTable<Ticket> ticketsTable;
	private int _selectedRow = -1;
	private Ticket? _selectedTicket = null;

	// TicketView attribute
	private List<Ticket>? _tickets;
	private List<Comment>? _comments;
	private User? _assignee;
	private Attachement? _attachement;

	protected override async Task OnInitializedAsync()
	{
		var project = await _projectsService.GetAllAsync();

		switch (project.IsSuccess)
		{
			case true when project.Item != null:
				Project = project.Item.Find(t => t.ProjectId == this.ProjectId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				_dialogService.Show<ErrorDialog>("Failed to fetch Project data from database", options);
				break;
		}

		var tickets = await _ticketsService.GetAllAsync();

		switch (tickets.IsSuccess)
		{
			case true when tickets.Item != null:
				// TODO: tady by šel použít pohled
				_tickets = tickets.Item.FindAll(t => t.ProjectId == this.ProjectId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				_dialogService.Show<ErrorDialog>("Failed to fetch Ticket data from database", options);
				break;
		}
	}

	private string OnTicketClicked(Ticket ticket, int rowNumber)
	{
		if (this._selectedRow == rowNumber)
		{
			this._selectedRow = -1;
			return string.Empty;
		}

		if (ticketsTable.SelectedItem == null || !ticketsTable.SelectedItem.Equals(ticket)) return string.Empty;

		this._selectedRow = rowNumber;
		this._selectedTicket = ticket;

		LoadTicketInfo(ticket);

		return "selected";
	}

	private async void LoadTicketInfo(Ticket ticket)
	{
		// TODO: tady by šly použít pohledy
		var comments = await _commentsService.GetAllAsync();

		switch (comments.IsSuccess)
		{
			case true when comments.Item != null:
				_comments = comments.Item.FindAll(c => c.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				_dialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var users = await _usersService.GetAllAsync();

		switch (users.IsSuccess)
		{
			case true when users.Item != null:
				_assignee = users.Item.Find(c => c.UserId == ticket.AssignedUserId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				_dialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var attachements = await _attachementsService.GetAllAsync();

		switch (attachements.IsSuccess)
		{
			case true when attachements.Item != null:
				_attachement = attachements.Item.Find(a => a.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				_dialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}
		
		StateHasChanged();
	}

	private void OnAddTicket()
	{
		// TODO: add Ticket
	}

	private void OnRemoveTicket()
	{
		// TODO: remove Ticket
	}
	
	private void OnCommentTicket()
	{
		// TODO: remove Ticket
	}
}
