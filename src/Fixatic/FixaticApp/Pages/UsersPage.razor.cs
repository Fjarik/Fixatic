using Microsoft.AspNetCore.Components;
using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using MudBlazor;
using Fixatic.DO.Types;

namespace FixaticApp.Pages
{
	public partial class UsersPage
	{
		[Inject] private IUsersService? UsersService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }
		
		[Inject] private ICurrentUserService? CurrentUserService { get; set; }

		[Inject] private NavigationManager? NavigationManager { get; set; }

		private List<User> _users = new();

		private User? _selectedUser;

		protected override async Task OnInitializedAsync()
		{
			var user = await CurrentUserService!.GetUserInfoAsync();
			if (!user.IsInGroup(UserGroupType.Admin))
			{
				NavigationManager!.NavigateTo("/");
				return;
			}

			await LoadUsersAsync();
		}

		private async Task LoadUsersAsync()
		{
			var usersRes = await UsersService!.GetAllAsync();
			if (!usersRes.IsSuccess)
				return;

			_users = usersRes.Item!;
		}

		private async Task UserSelectedAsync()
		{
			if (_selectedUser == null)
			{
				return;
			}

			await ShowUserDialogAsync(_selectedUser);
		}

		private async Task OnAddClickedAsync()
		{
			var newUser = new User
			{
				UserId = DB.IgnoredID,
				Created = DateTime.Now,
				IsEnabled = true,
				Email = "",
				Firstname = "",
				Lastname = "",
				Password = "",
				Phone = ""
			};

			await ShowUserDialogAsync(newUser);
		}

		private async Task ShowUserDialogAsync(User inputUser)
		{
			inputUser.Password = "";

			var parameters = new DialogParameters { { "User", inputUser } };

			var options = new DialogOptions { MaxWidth = MaxWidth.Medium, FullWidth = true };
			var dialog = DialogService!.Show<UserEditDialog>("Edit user", parameters, options);
			var result = await dialog.Result;

			if (!result.Cancelled)
			{
				await LoadUsersAsync();
				StateHasChanged();
			}
		}

	}
}