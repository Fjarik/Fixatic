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
			_selectedTicketId = _tickets.Any(t => t.TicketId == RouteSelectedTicketId) ? ((int)RouteSelectedTicketId) : -1; ;
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

}