using Microsoft.AspNetCore.Components;
using MudBlazor;
using Fixatic.Types;
using Fixatic.Services;

namespace FixaticApp.Components.Tickets
{
	public partial class UserAssignmentDialog
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public int TicketID { get; set; } = -1;

		[Inject] private IUsersService? UsersService { get; set; }

		[Inject] private ITicketsService? TicketsService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }

		private List<BasicUserInfo> Users { get; set; } = new();
		private int SelectedUserId { get; set; } = -1;

		protected override async Task OnInitializedAsync()
		{
			if (TicketID < 1)
			{
				DialogService!.Show<ErrorDialog>("Error");
				Cancel();
				return;
			}

			var usersRes = await UsersService!.GetPossibleTicketAssigneesAsync(TicketID);
			if (!usersRes.IsSuccess)
			{
				DialogService!.Show<ErrorDialog>("Couldn't fetch users from DB");
				Cancel();
				return;
			}

			Users = usersRes.Item ?? new List<BasicUserInfo>();

			SelectedUserId = Users.FirstOrDefault()?.UserId ?? -1;
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

		private async Task SubmitAsync()
		{
			if (SelectedUserId < 1)
				return;

			var res = await TicketsService!.SetAssigneeAsync(TicketID, SelectedUserId);

			MudDialog!.Close(DialogResult.Ok(true));
		}
	}
}