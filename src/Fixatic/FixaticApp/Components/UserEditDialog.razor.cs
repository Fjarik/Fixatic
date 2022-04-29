using Fixatic.DO.Types;
using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Types;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Components
{
	public partial class UserEditDialog
	{
		[Parameter] public User? User { get; set; }

		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Inject] private ICurrentUserService? CurrentUserService { get; set; }
		[Inject] private IUsersService? UsersService { get; set; }
		[Inject] private IDialogService? DialogService { get; set; }

		private MudForm? _form;
		private CurrentUser? CurrentUser;
		private bool FormValid { get; set; } = false;
		private bool UpdatePassword { get; set; } = false;

		// Is this dialog for creating a new user or just for editing ?
		private bool IsCreate => User?.UserId == DB.IgnoredID;
		private bool CanDelete => CurrentUser != null && !IsCreate && CurrentUser?.UserId != User?.UserId;

		// The validation rules (overkill, I know, but very fluent):
		private readonly FluentValueValidator<string> _emailValidator = new(x => x
			.NotEmpty()
			.Length(1, 100)
			.EmailAddress());

		private readonly FluentValueValidator<string> _nameValidator = new(x => x
			.NotEmpty()
			.Length(1, 50));

		private readonly FluentValueValidator<string> _passwordValidator = new(x => x
			.NotEmpty()
			.Length(5, 50));

		private readonly FluentValueValidator<string> _phoneValidator = new(x => x
			.NotEmpty()
			.Length(10, 50)
			.Must(phone => phone.StartsWith("+"))
			.Must(phone => !string.IsNullOrWhiteSpace(phone) && phone.ToCharArray()[1..].All(char.IsDigit))
			.WithMessage("Phone must start with '+' and then only contain digits"));

		protected override async Task OnInitializedAsync()
		{
			CurrentUser = await CurrentUserService!.GetUserInfoAsync();
			await base.OnInitializedAsync();
		}

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
			await _form!.Validate();
			if (User == null || !_form.IsValid)
				return;

			if (IsCreate || UpdatePassword)
			{
				var updateRes = await UsersService!.CreateOrUpdateAsync(User);
				if (!updateRes.IsSuccess)
				{
					DialogService!.Show<ErrorDialog>("Failed to update user data");
				}
				Close();
				return;
			}

			var res = await UsersService!.UpdateSansPasswordAsync(User);
			if (!res.IsSuccess)
			{
				DialogService!.Show<ErrorDialog>("Failed to update user data");
			}
			Close();
		}

		private async Task DeleteAsync()
		{
			if (User == null || !CanDelete)
				return;

			var updateRes = await UsersService!.DeleteAsync(User.UserId);
			if (!updateRes.IsSuccess)
			{
				DialogService!.Show<ErrorDialog>("Failed to delete user data");
			}
			Close();
		}

	}
}