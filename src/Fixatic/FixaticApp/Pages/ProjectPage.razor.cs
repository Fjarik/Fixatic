using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Pages;

public partial class ProjectPage
{
	[Parameter] public int RouteProjectId { get; set; }

	[Parameter] public int? RouteSelectedTicketId { get; set; }

	[Parameter] public Project? Project { get; set; }

	[Inject] private IProjectsService? ProjectsService { get; set; }

	[Inject] private IDialogService? DialogService { get; set; }

	[Inject] private ITicketsService? TicketsService { get; set; }

	[Inject] private ICommentsService? CommentsService { get; set; }

	[Inject] private IUsersService? UsersService { get; set; }

	[Inject] private ICurrentUserService? CurrentUserService { get; set; }

	[Inject] private IAttachementsService? AttachementsService { get; set; }

	private MudTable<Ticket>? _ticketsTable;
	private int _selectedRow = -1;
	private Ticket? _selectedTicket;

	// TicketView attribute
	private List<Ticket>? _tickets;
	private List<Comment>? _comments;
	private User? _assignee;
	private Attachement? _attachement;
	private bool _isFollowed;

	protected override async Task OnInitializedAsync()
	{
		var project = await ProjectsService!.GetAllAsync();

		switch (project.IsSuccess)
		{
			case true when project.Item != null:
				Project = project.Item.Find(t => t.ProjectId == this.RouteProjectId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				DialogService!.Show<ErrorDialog>("Failed to fetch Project data from database", options);
				break;
		}

		var tickets = await TicketsService!.GetAllAsync();

		switch (tickets.IsSuccess)
		{
			case true when tickets.Item != null:
				// TODO: tady by šel použít pohled
				_tickets = tickets.Item.FindAll(t => t.ProjectId == this.RouteProjectId);

				if (RouteSelectedTicketId != null)
				{
					_selectedTicket = tickets.Item.Find(t => t.TicketId == RouteSelectedTicketId);
				}

				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				DialogService!.Show<ErrorDialog>("Failed to fetch Ticket data from database", options);
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

		if (_ticketsTable!.SelectedItem == null || !_ticketsTable.SelectedItem.Equals(ticket)) return string.Empty;

		this._selectedRow = rowNumber;
		this._selectedTicket = ticket;

		LoadTicketInfo(ticket);

		return "selected";
	}

	private async void LoadTicketInfo(Ticket ticket)
	{
		_isFollowed = (await TicketsService!.IsFollowedAsync(ticket.TicketId))?.Item == true;

		// TODO: tady by šly použít pohledy
		var comments = await CommentsService!.GetAllAsync();

		switch (comments.IsSuccess)
		{
			case true when comments.Item != null:
				_comments = comments.Item.FindAll(c => c.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				DialogService!.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var users = await UsersService!.GetAllAsync();

		switch (users.IsSuccess)
		{
			case true when users.Item != null:
				_assignee = users.Item.Find(c => c.UserId == ticket.AssignedUserId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				DialogService!.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		var attachements = await AttachementsService!.GetAllAsync();

		switch (attachements.IsSuccess)
		{
			case true when attachements.Item != null:
				_attachement = attachements.Item.Find(a => a.TicketId == ticket.TicketId);
				break;
			case false:
				var options = new DialogOptions {CloseOnEscapeKey = true};
				DialogService!.Show<ErrorDialog>("Failed to fetch Ticket comments from database", options);
				break;
		}

		StateHasChanged();
	}

	private void OnAddTicket()
	{
		// TODO: add Ticket
	}

	private async Task OnRemoveTicketAsync()
	{
		if (_selectedTicket == null || _tickets == null)
			return;

		// TODO: check result
		var result = await TicketsService!.DeleteAsync(_selectedTicket.TicketId);

		_tickets.Remove(_selectedTicket);
		_selectedTicket = null;
		StateHasChanged();
	}

	private async Task OnCommentTicketAsync()
	{
		if (_selectedTicket == null)
			return;

		var options = new DialogOptions {CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true};
		var dialog = DialogService!.Show<TextInputDialog>("Add comment", options);
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (textContent, isInternal) = ((string, bool))result.Data;
			var curentUser = await CurrentUserService!.GetUserInfoAsync();

			if (string.IsNullOrEmpty(textContent))
			{
				return;
			}

			var comment = new Comment
			{
				CommentId = -1,
				TicketId = _selectedTicket.TicketId,
				UserId = curentUser.UserId,
				Content = textContent,
				Created = DateTime.Now,
				IsInternal = isInternal
			};

			await CommentsService!.CreateOrUpdateAsync(comment);
			// TODO: check errors
		}
	}

	private async Task OnFollowTicketClick()
	{
		if (_selectedTicket == null)
			return;

		var result = await TicketsService!.SetFollowTicketAsync(_selectedTicket!.TicketId, !_isFollowed);
		if (result.IsSuccess)
		{
			_isFollowed = result.Item;
		}
	}
}