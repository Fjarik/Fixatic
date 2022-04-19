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
using Fixatic.DO.Types;
using Fixatic.Services;

namespace FixaticApp.Components.Tickets
{
	public partial class TicketAttachmentDialog
	{
		private static string DefaultDragClass = "relative rounded-lg border-2 border-dashed pa-4 mt-4 mud-width-full mud-height-full";

		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public int TicketID { get; set; } = -1;

		[Inject] private IAttachementsService? AttachementsService { get; set; }
		[Inject] private IDialogService? DialogService { get; set; }

		private string AltTxt { get; set; } = string.Empty;
		private bool Clearing { get; set; } = false;

		private string DragClass = DefaultDragClass;
		private IBrowserFile? _file;

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

		private void Close()
		{
			MudDialog!.Close(DialogResult.Ok(true));
		}

		private async Task SubmitAsync()
		{
			if (_file == null || TicketID < 1)
				return;

			var max = 1024 * 1024 * 10;

			if (_file.Size > max)
				return;

			byte[] content;
			using (var memoryStream = new MemoryStream())
			{
				await _file.OpenReadStream(max).CopyToAsync(memoryStream);
				content = memoryStream.ToArray();
			}

			var att = new Attachement
			{
				AttachementId = DB.IgnoredID,
				TicketId = TicketID,
				CommentId = null,
				AlternativeText = AltTxt,
				Name = _file.Name,
				Type = _file.ContentType,
				Size = (int)_file.Size,
				Content = content
			};

			var updateRes = await AttachementsService!.CreateOrUpdateAsync(att);
			if (!updateRes.IsSuccess)
			{
				DialogService!.Show<ErrorDialog>("Failed to upload attachement");
			}
			Close();
		}

		private void OnInputFileChanged(InputFileChangeEventArgs e)
		{
			ClearDragClass();
			_file = e.File;
		}

		private async Task Clear()
		{
			Clearing = true;
			_file = null; ;
			ClearDragClass();
			await Task.Delay(100);
			Clearing = false;
		}

		private void SetDragClass()
		{
			DragClass = $"{DefaultDragClass} mud-border-primary";
		}

		private void ClearDragClass()
		{
			DragClass = DefaultDragClass;
		}
	}
}