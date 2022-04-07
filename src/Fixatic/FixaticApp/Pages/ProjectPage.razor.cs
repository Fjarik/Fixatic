using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Pages;

public partial class ProjectPage
{
	[Parameter] public int ProjectId { get; set; }

	[Parameter] public Project? Project { get; set; }


	[Inject]
	private IProjectsService ProjectsService { get; set; }

	[Inject]
	private IDialogService DialogService { get; set; }

	[Inject]
	private ITicketsService TicketsService { get; set; }

	[Inject]
	private ICommentsService CommentsService { get; set; }

	[Inject]
	private IUsersService UsersService { get; set; }

	[Inject]
	private IAttachementsService _attachementsService { get; set; }

	private MudTable<Ticket> ticketsTable;
	private int _selectedRow = -1;
	private Ticket? _selectedTicket = null;

	// TicketView attribute
	private List<Ticket>? _tickets;
	private List<Comment>? _comments;
	private User? _assignee;
	private Attachement? _attachement;
	private bool _isFollowed = false;

	protected override async Task OnInitializedAsync()
	{
		var project = await ProjectsService.GetAllAsync();

		switch (project.IsSuccess)
		{
			case true when project.Item != null:
				Project = project.Item.Find(t => t.ProjectId == this.ProjectId);
				break;
			case false:
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService.Show<ErrorDialog>("Failed to fetch Project data from database", options);
				break;
		}

		var tickets = await TicketsService.GetAllAsync();

		switch (tickets.IsSuccess)
		{
			case true when tickets.Item != null:
				// TODO: tady by šel použít pohled
				_tickets = tickets.Item.FindAll(t => t.ProjectId == this.ProjectId);
				break;
			case false:
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService.Show<ErrorDialog>("Failed to fetch Ticket data from database", options);
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
		_isFollowed = (await TicketsService.IsFollowedAsync(ticket.TicketId))?.Item == true;

		// TODO: tady by šly použít pohledy
		var comments = await CommentsService.GetAllAsync();

		switch (comments.IsSuccess)
		{
			case true when comments.Item != null:
				_comments = comments.Item.FindAll(c => c.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var users = await UsersService.GetAllAsync();

		switch (users.IsSuccess)
		{
			case true when users.Item != null:
				_assignee = users.Item.Find(c => c.UserId == ticket.AssignedUserId);
				break;
			case false:
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var attachements = await _attachementsService.GetAllAsync();

		switch (attachements.IsSuccess)
		{
			case true when attachements.Item != null:
				_attachement = attachements.Item.Find(a => a.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
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
		if (_selectedTicket == null)
			return;
		
		// TODO: remove Ticket
	}

	private void OnCommentTicket()
	{
		if (_selectedTicket == null)
			return;
		
		// TODO: comment Ticket
	}

	private async Task OnFollowTicketClick()
	{
		if (_selectedTicket == null)
			return;

		var result = await TicketsService.SetFollowTicketAsync(_selectedTicket!.TicketId, !_isFollowed);
		if (result.IsSuccess)
		{
			_isFollowed = result.Item;
		}
	}

}