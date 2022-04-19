using Microsoft.AspNetCore.Components;
using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using MudBlazor;

namespace FixaticApp.Pages
{
	public partial class UsersPage
	{
		[Inject] private IUsersService? UsersService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }

		private List<User> _users = new();

		private User? _selectedUser;

		protected override async Task OnInitializedAsync()
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

			await ShowUserDialogAsync(_selectedUser, false);
		}

		private async Task OnAddClickedAsync()
		{
			var newUser = new User
			{
				UserId = -1,
				Created = DateTime.Now,
				IsEnabled = true,
				Email = "",
				Firstname = "",
				Lastname = "",
				Password = "",
				Phone = ""
			};

			await ShowUserDialogAsync(newUser, true);
		}

		private async Task ShowUserDialogAsync(User inputUser, bool isCreate)
		{
			inputUser.Password = "";

			var parameters = new DialogParameters { { "User", inputUser }, { "IsCreate", isCreate } };

			var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
			var dialog = DialogService!.Show<UserEditDialog>("Edit user", parameters, options);
			var result = await dialog.Result;

			if (!result.Cancelled)
			{
				await UpdateUserDataAsync(result, isCreate);
				StateHasChanged();
			}
		}

		private async Task UpdateUserDataAsync(DialogResult result, bool isCreate)
		{
			var (user, deleteUser, updatePassword) = ((User, bool, bool))result.Data;

			if (deleteUser)
			{
				user.Password = " ";
				var updateRes = await UsersService!.DeleteAsync(user.UserId);
				if (!updateRes.IsSuccess)
				{
					DialogService!.Show<ErrorDialog>("Failed to delete user data");
				}
			}
			else if (isCreate || updatePassword)
			{
				var updateRes = await UsersService!.CreateOrUpdateAsync(user);
				if (!updateRes.IsSuccess)
				{
					DialogService!.Show<ErrorDialog>("Failed to update user data");
				}
			}
			else
			{
				var updateRes = await UsersService!.UpdateSansPasswordAsync(user);
				if (!updateRes.IsSuccess)
				{
					DialogService!.Show<ErrorDialog>("Failed to update user data");
				}
			}
		}
	}
}