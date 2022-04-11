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
using FixaticApp.Components;
using Fixatic.Types;

namespace FixaticApp.Components.Tickets
{
	public partial class TicketsTable
	{
		[Parameter]
		public List<Ticket> Tickets { get; set; } = new List<Ticket>();

		[Parameter]
		public EventCallback<Ticket> OnTicketSelect { get; set; }

		private Ticket? _selectedTicket;

		private string GetText(DateTime? dateTime)
		{
			if (dateTime == null)
				return "Never";

			return dateTime.Value.ToString("dd/MM/yyyy HH:mm");
		}

		private async Task OnTicketClick(TableRowClickEventArgs<Ticket> args)
		{
			if (args?.Item == null)
				return;

			if (OnTicketSelect.HasDelegate)
			{
				await OnTicketSelect.InvokeAsync(args.Item);
			}
		}
	}
}