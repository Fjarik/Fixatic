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

	[Inject] private ICurrentUserService? CurrentUserService { get; set; }


	private FullTicket? _selectedTicket;

	// TicketView attribute
	private List<FullTicket> _tickets = new();
	private bool _isFollowed;

	protected override async Task OnInitializedAsync()
	{
		var projectRes = await ProjectsService!.GetByIdAsync(this.RouteProjectId);
		if (!projectRes.IsSuccess || projectRes.Item == null)
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			DialogService!.Show<ErrorDialog>("Failed to fetch Project data from database", options);
			return;
		}

		Project = projectRes.Item;

		var ticketsRes = await TicketsService!.GetByProjectAsync(this.RouteProjectId);
		if (!ticketsRes.IsSuccess)
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			DialogService!.Show<ErrorDialog>("Failed to fetch Ticket data from database", options);
			return;
		}

		_tickets = ticketsRes.Item!;
		if (RouteSelectedTicketId != null)
		{
			_selectedTicket = _tickets.FirstOrDefault(t => t.TicketId == RouteSelectedTicketId);
		}

	}

	private void OnAddTicket()
	{
		// TODO: add Ticket
	}

	private async Task OnRemoveTicketAsync()
	{
		if (_selectedTicket == null)
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

		var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
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