using Microsoft.AspNetCore.Components;
using MudBlazor;
using Fixatic.Types;
using Fixatic.Services;

namespace FixaticApp.Components.Tickets
{
	public partial class UserAssignmentDialog
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

		[Parameter] public int TicketID { get; set; } = -1;

		[Parameter] public List<User>? Users { get; set; }

		[Parameter] public User? SelectedUser { get; set; }

		[Inject] private IUsersService? UsersService { get; set; }

		[Inject] private ITicketsService? TicketsService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }

		protected override async Task OnInitializedAsync()
		{
			if (TicketID == -1)
			{
				DialogService!.Show<ErrorDialog>("Error");
				Cancel();
			}

			var usersRes = await UsersService!.GetPossibleTicketAssigneesAsync(TicketID);
			if (!usersRes.IsSuccess)
			{
				DialogService!.Show<ErrorDialog>("Couldn't fetch users from DB");
				Cancel();
			}

			Users = usersRes.Item;

			if (Users!.Count > 0)
			{
				SelectedUser = Users[0];
			}
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

		private async Task SubmitAsync()
		{
			var res = await TicketsService!.SetAssigneeAsync(TicketID, SelectedUser!.UserId);

			MudDialog!.Close(DialogResult.Ok(true));
		}
	}
}