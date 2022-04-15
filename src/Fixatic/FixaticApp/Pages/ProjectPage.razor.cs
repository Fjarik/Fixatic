using Fixatic.DO.Types;
using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using FixaticApp.Components.Tickets;
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

	[Inject] private NavigationManager? NavigationManager { get; set; }


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

		await LoadTicketsAsync();
	}

	private async Task LoadTicketsAsync()
	{
		var ticketsRes = await TicketsService!.GetByProjectAsync(this.RouteProjectId);
		if (!ticketsRes.IsSuccess)
		{
			var options = new DialogOptions { CloseOnEscapeKey = true };
			DialogService!.Show<ErrorDialog>("Failed to fetch Ticket data from database", options);
			return;
		}

		_tickets = ticketsRes.Item!;
	}

	protected override async Task OnParametersSetAsync()
	{
		if (RouteSelectedTicketId == null)
		{
			_selectedTicket = null;
		}
		else
		{
			_selectedTicket = _tickets.FirstOrDefault(t => t.TicketId == RouteSelectedTicketId);
			if (_selectedTicket != null)
			{
				_isFollowed = (await TicketsService!.IsFollowedAsync(_selectedTicket.TicketId)).Item == true;
			}
		}

	}

	private void OnTicketSelect(TableRowClickEventArgs<FullTicket> args)
	{
		if (args?.Item == null)
			return;

		NavigationManager!.NavigateTo($"/project/{RouteProjectId}/{args.Item.TicketId}");
	}

	private async Task OnAddTicket()
	{
		var ticketRes = await TicketsService!.GetByIdAsync(DB.IgnoredID);
		if (!ticketRes.IsSuccess || ticketRes.Item == null)
			return;

		var ticket = ticketRes.Item;
		ticket.ProjectId = RouteProjectId;

		await EditTicketAsync(ticket);
	}

	private async Task OnEditClick()
	{
		if (_selectedTicket == null)
			return;

		await EditTicketAsync(_selectedTicket);
	}

	private async Task EditTicketAsync(FullTicket ticket)
	{
		var parameters = new DialogParameters
		{
			{ "Ticket", ticket }
		};
		var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
		var dialog = DialogService!.Show<TicketEditDialog>("Ticket", parameters, options);
		var result = await dialog.Result;
		if (!result.Cancelled)
		{
			await LoadTicketsAsync();
			StateHasChanged();
		}
	}

	private async Task OnRemoveTicketAsync()
	{
		if (_selectedTicket == null)
			return;

		await TicketsService!.DeleteAsync(_selectedTicket.TicketId);

		_tickets.Remove(_selectedTicket);
		_selectedTicket = null;
		StateHasChanged();
	}

	private async Task OnCommentTicketAsync()
	{
		if (_selectedTicket == null)
			return;

		var original = _selectedTicket;

		_selectedTicket = null;
		var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
		var dialog = DialogService!.Show<TextInputDialog>("Add comment", options);
		var result = await dialog.Result;

		if (!result.Cancelled)
		{
			var (textContent, isInternal) = ((string, bool))result.Data;
			var curentUser = await CurrentUserService!.GetUserInfoAsync();

			if (string.IsNullOrEmpty(textContent))
			{
				_selectedTicket = original;
				return;
			}

			var comment = new Comment
			{
				CommentId = -1,
				TicketId = original.TicketId,
				UserId = curentUser.UserId,
				Content = textContent,
				Created = DateTime.Now,
				IsInternal = isInternal
			};

			await CommentsService!.CreateOrUpdateAsync(comment);
			// TODO: check errors
		}

		_selectedTicket = original;
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