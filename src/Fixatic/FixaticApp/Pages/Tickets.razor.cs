using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.JSInterop;
using MudBlazor;
using FixaticApp;
using FixaticApp.Shared;
using Fixatic.Services;
using Fixatic.Types;

namespace FixaticApp.Pages
{
	public partial class Tickets
	{
		[Parameter]
		public int? TicketId { get; set; } = null;

		[Inject] private ITicketsService? TicketsService { get; set; }

		[Inject] private NavigationManager? NavigationManager { get; set; }

		private List<FullTicket> _tickets = new();
		private List<Ticket> BaseTickets => _tickets.Cast<Ticket>().ToList();
		private FullTicket? _selectedTicket;

		protected override async Task OnParametersSetAsync()
		{
			if (TicketId.HasValue)
			{
				var ticketRes = await TicketsService!.GetByIdAsync(TicketId.Value);
				if (ticketRes.IsSuccess && ticketRes.Item != null)
				{
					_selectedTicket = ticketRes.Item;
					return;
				}
			}
			_selectedTicket = null;
			var ticketsRes = await TicketsService!.GetAllAsync();
			if (ticketsRes.IsSuccess)
			{
				_tickets = ticketsRes.Item!;
			}
		}

		private void TicketSelected(Ticket ticket)
		{
			NavigationManager!.NavigateTo("/tickets/" + ticket.TicketId);
		}
	}
}