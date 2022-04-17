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
	[Inject] private NavigationManager? NavigationManager { get; set; }
	[Inject] private ICurrentUserService? CurrentUserService { get; set; }

	private int _selectedTicketId = -1;
	private List<FullTicket> _tickets = new();

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

	protected override void OnParametersSet()
	{
		if (RouteSelectedTicketId == null)
		{
			_selectedTicketId = -1;
		}
		else
		{
			_selectedTicketId = _tickets.Any(t => t.TicketId == RouteSelectedTicketId)
				? ((int)RouteSelectedTicketId)
				: -1;
			;
		}
	}

	private void OnTicketSelect(TableRowClickEventArgs<FullTicket> args)
	{
		if (args?.Item == null)
			return;

		NavigationManager!.NavigateTo($"/project/{RouteProjectId}/{args.Item.TicketId}");
	}

	private async Task OnTicketRemoved()
	{
		_selectedTicketId = -1;
		await LoadTicketsAsync();
		StateHasChanged();
	}

	private async Task OnAddTicket()
	{
		if (Project == null)
			return;

		var ticketRes = await TicketsService!.GetByIdAsync(DB.IgnoredID);
		if (!ticketRes.IsSuccess || ticketRes.Item == null)
			return;

		var ticket = ticketRes.Item;
		ticket.ProjectId = Project.ProjectId;

		await EditTicketAsync(ticket);
	}

	private async Task EditTicketAsync(FullTicket ticket)
	{
		var parameters = new DialogParameters { { "Ticket", ticket } };
		var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
		var dialog = DialogService!.Show<TicketEditDialog>("Ticket", parameters, options);
		var result = await dialog.Result;
		if (!result.Cancelled)
		{
			await LoadTicketsAsync();
			StateHasChanged();
		}
	}
}