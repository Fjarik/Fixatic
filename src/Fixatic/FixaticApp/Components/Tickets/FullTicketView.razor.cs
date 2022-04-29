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
using Fixatic.DO.Types;

namespace FixaticApp.Components.Tickets
{
	public partial class FullTicketView
	{
		[Parameter] public int TicketId { get; set; } = -1;

		[Parameter] public EventCallback OnRemoved { get; set; }

		[CascadingParameter(Name = "CurrentUser")]
		public CurrentUser? CurrentUser { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }
		[Inject] private ICommentsService? CommentsService { get; set; }
		[Inject] private ITicketsService? TicketsService { get; set; }

		private FullTicket? Model;
		private int _prevId = 0;
		private bool _isFollowed;

		protected override async Task OnParametersSetAsync()
		{
			if (TicketId < 1)
				return;
			if (_prevId == TicketId)
				return;
			_prevId = TicketId;

			await LoadModelAsync();
		}

		private async Task LoadModelAsync()
		{
			Model = null;
			StateHasChanged();

			var ticketRes = await TicketsService!.GetByIdAsync(TicketId);
			if (!ticketRes.IsSuccess || ticketRes.Item == null)
				return;

			Model = ticketRes.Item;

			_isFollowed = (await TicketsService!.IsFollowedAsync(Model.TicketId)).Item == true;
		}

		private async Task OnEditClick()
		{
			if (Model == null)
				return;

			await EditTicketAsync(Model);
		}

		private async Task EditTicketAsync(FullTicket ticket)
		{
			var parameters = new DialogParameters {{"Ticket", ticket}};
			var options = new DialogOptions {CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true};
			var dialog = DialogService!.Show<TicketEditDialog>("Ticket", parameters, options);
			var result = await dialog.Result;
			if (!result.Cancelled)
			{
				await LoadModelAsync();
			}
		}

		private async Task OnRemoveTicketAsync()
		{
			if (Model == null || CurrentUser == null)
				return;

			if (CurrentUser.UserId != Model.CreatorId && !CurrentUser.IsInGroup(UserGroupType.Admin))
			{
				DialogService!.Show<ErrorDialog>("Only the creator can delete the ticket");
				return;
			}

			var confirmDialog = DialogService!.Show<ConfirmDialog>("Delete the ticket?");
			var confirmRes = await confirmDialog.Result;
			if (confirmRes.Cancelled)
			{
				return;
			}

			await TicketsService!.DeleteAsync(Model.TicketId);

			if (OnRemoved.HasDelegate)
			{
				await OnRemoved.InvokeAsync();
			}
		}

		private async Task OnCommentTicketAsync()
		{
			if (Model == null)
				return;

			var original = Model;

			Model = null;
			var parameters = new DialogParameters {{"AllowInternal", CurrentUser!.IsInternal()}};
			var options = new DialogOptions {CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true};
			var dialog = DialogService!.Show<CommentDialog>("Add comment", parameters, options);
			var result = await dialog.Result;

			if (!result.Cancelled)
			{
				var (textContent, isInternal) = ((string, bool))result.Data;

				if (string.IsNullOrEmpty(textContent))
				{
					Model = original;
					return;
				}

				var comment = new Comment
				{
					CommentId = -1,
					TicketId = original.TicketId,
					UserId = CurrentUser!.UserId,
					Content = textContent,
					Created = DateTime.Now,
					IsInternal = isInternal
				};

				await CommentsService!.CreateOrUpdateAsync(comment);
			}

			Model = original;
		}

		private async Task OnFollowTicketClick()
		{
			if (Model == null)
				return;

			var result = await TicketsService!.SetFollowTicketAsync(Model!.TicketId, !_isFollowed);
			if (result.IsSuccess)
			{
				_isFollowed = result.Item;
			}
		}

		private async Task OnCumPropsClick()
		{
			var parameters = new DialogParameters {{"TicketID", TicketId}};
			var options = new DialogOptions {MaxWidth = MaxWidth.Medium, FullWidth = true};
			var dialog = DialogService!.Show<TicketPropertiesDialog>("Ticket properties", parameters, options);
			await dialog.Result;
			await LoadModelAsync();
		}

		private async Task OnAddAttachmentsClick()
		{
			var parameters = new DialogParameters {{"TicketID", TicketId}};
			var options = new DialogOptions {MaxWidth = MaxWidth.Medium, FullWidth = true};
			var dialog = DialogService!.Show<TicketAttachmentDialog>("Ticket attachment", parameters, options);
			await dialog.Result;
			await LoadModelAsync();
		}

		private async Task OnAssigneeClickAsync()
		{
			var parameters = new DialogParameters {{"TicketID", TicketId}};
			var options = new DialogOptions {MaxWidth = MaxWidth.Medium, FullWidth = true};
			var dialog = DialogService!.Show<UserAssignmentDialog>("Assign user", parameters, options);
			await dialog.Result;
			await LoadModelAsync();
		}
	}
}