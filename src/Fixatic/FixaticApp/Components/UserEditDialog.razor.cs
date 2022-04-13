using Fixatic.Types;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Components
{
	public partial class UserEditDialog
	{
		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public User? User { get; set; } 

		private void Submit()
		{
			MudDialog!.Close(DialogResult.Ok((User, false)));
		}

		private void Delete()
		{
			MudDialog!.Close(DialogResult.Ok((User, true)));
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}
	}
}