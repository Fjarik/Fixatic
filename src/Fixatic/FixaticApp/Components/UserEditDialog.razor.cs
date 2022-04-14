using Fixatic.Types;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Components
{
	public partial class UserEditDialog
	{
		[CascadingParameter] private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		// Is this dialog for creating a new user or just for editing ?
		public bool IsCreate { get; set; }

		[Parameter] public bool UpdatePassword { get; set; } = false;

		[Parameter] public User? User { get; set; }

		[Parameter] public bool FormValid { get; set; }

		/// <returns>
		/// Tuple (User user, bool deleteUser, bool updateUserPassword)  
		/// </returns>
		private void Submit()
		{
			if (FormValid)
			{
				MudDialog!.Close(DialogResult.Ok((User, false, UpdatePassword)));
			}
		}

		private void Delete()
		{
			if (!IsCreate)
			{
				MudDialog!.Close(DialogResult.Ok((User, true, UpdatePassword)));
			}
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

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

		// TODO: fix error with empty number... I thought NotEmpty() would take care of that...
		private readonly FluentValueValidator<string> _phoneValidator = new(x => x
			.NotEmpty()
			.Length(10, 50)
			.Must(phone => phone.StartsWith("+"))
			.Must(phone => phone.ToCharArray()[1..].All(char.IsDigit))
			.WithMessage("Phone must start with '+' and then only contain digits"));

		/// <summary>
		/// A glue class to make it easy to define validation rules for single values using FluentValidation
		/// </summary>
		public class FluentValueValidator<T> : AbstractValidator<T>
		{
			public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
			{
				rule(RuleFor(x => x));
			}

			private IEnumerable<string> ValidateValue(T arg)
			{
				var result = Validate(arg);
				if (result.IsValid)
					return new string[0];
				return result.Errors.Select(e => e.ErrorMessage);
			}

			public Func<T, IEnumerable<string>> Validation => ValidateValue;
		}
	}
}