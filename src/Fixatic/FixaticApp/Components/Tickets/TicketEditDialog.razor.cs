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
using FixaticApp.Types;
using FluentValidation;
using Fixatic.Services;

namespace FixaticApp.Components.Tickets
{
	public partial class TicketEditDialog
	{
		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public FullTicket? Ticket { get; set; }

		[Inject]
		private ITicketsService? TicketsService { get; set; }

		private MudForm? _form;
		private bool FormValid = false;
		private bool IsCreate => Ticket?.TicketId < 1;
		private List<TicketVisibility> _ticketVisibilities = new();

		private readonly FluentValueValidator<string> _titleValidator = new(x => x
			.NotEmpty()
			.Length(1, 50));

		private readonly FluentValueValidator<string> _contentValidator = new(x => x
			.NotEmpty()
			.Length(1, 500));

		protected override async Task OnInitializedAsync()
		{
			_ticketVisibilities = await TicketsService!.GetAvailableVisiblityAsync();
			
			await base.OnInitializedAsync();
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

		private async Task Submit()
		{
			await _form!.Validate();
			if (!FormValid || Ticket == null)
				return;

			var res = await TicketsService!.CreateOrUpdateAsync(Ticket);
			MudDialog!.Close(DialogResult.Ok(Ticket));
		}
	}
}