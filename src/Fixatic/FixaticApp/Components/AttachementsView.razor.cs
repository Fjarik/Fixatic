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

namespace FixaticApp.Components
{
	public partial class AttachementsView
	{
		[CascadingParameter(Name = "CurrentUser")]
		public CurrentUser? CurrentUser { get; set; }

		[Parameter]
		public int TicketID { get; set; } = -1;

		[Inject] private IAttachementsService? AttachementsService { get; set; }


		private List<Attachement> _attachements = new();
		private bool CanDelete => CurrentUser != null && CurrentUser.IsInternal();

		private int _prevId = 0;
		protected override async Task OnParametersSetAsync()
		{
			if (TicketID < 1)
				return;
			if (_prevId == TicketID)
				return;
			_prevId = TicketID;

			await LoadAttachementsAsync();
		}

		private async Task LoadAttachementsAsync()
		{
			var res = await AttachementsService!.GetByTicketAsync(TicketID);
			if (!res.IsSuccess)
				return;
			_attachements = res.Item ?? new List<Attachement>();
		}

		private async Task DeleteAsync(int attachmentId)
		{
			if (!CanDelete)
				return;

			await AttachementsService!.DeleteAsync(attachmentId);
			_attachements = _attachements.Where(x => x.AttachementId != attachmentId).ToList();
		}

		private static string GetSrc(Attachement attachement)
		{
			var base64 = Convert.ToBase64String(attachement.Content);
			return string.Format("data:{0};base64,{1}", attachement.Type, base64);
		}

	}
}