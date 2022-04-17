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
using Fixatic.Types;
using Fixatic.Types.Extensions;
using FixaticApp.Components;
using FixaticApp.Types;
using Fixatic.Services;

namespace FixaticApp.Components.Tickets
{
	public partial class TicketPropertiesDialog
	{
		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public int TicketID { get; set; } = -1;

		[Inject] private ITicketsService? TicketsService { get; set; }

		private int _prevId = -1;
		private List<FullTicketProperty> _properties = new();

		protected override async Task OnParametersSetAsync()
		{
			if (TicketID < 1)
				return;
			if (_prevId == TicketID)
				return;
			_prevId = TicketID;

			await LoadPropertiesAsync();
		}

		private async Task LoadPropertiesAsync()
		{
			var propRes = await TicketsService!.GetCustomPropertiesAsync(TicketID);
			if (!propRes.IsSuccess)
			{
				Close();
				return;
			}
			_properties = propRes.Item ?? new List<FullTicketProperty>();
		}

		private async Task SelectedChanged(FullTicketPropertyOption option)
		{
			if (option == null)
				return;

			if (option.IsSelected)
			{
				await TicketsService!.RemovePropertyOptionAsync(TicketID, option.Value.CustomPropertyOptionId);
			}
			else
			{
				await TicketsService!.AddPropertyOptionAsync(TicketID, option.Value.CustomPropertyOptionId);
			}

			await LoadPropertiesAsync();
		}


		private void Close()
		{
			MudDialog!.Cancel();
		}

	}
}