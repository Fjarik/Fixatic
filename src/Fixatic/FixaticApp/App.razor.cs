using Microsoft.AspNetCore.Components;
using FixaticApp.Types;

namespace FixaticApp
{
	public partial class App
	{
		[Parameter]
		public string AntiforgeryToken { get; set; } = string.Empty;

		[Inject]
		private TokenProvider? TokenProvider { get; set; }

		protected override Task OnInitializedAsync()
		{
			TokenProvider!.AntiforgeryToken = AntiforgeryToken;
			return base.OnInitializedAsync();
		}
	}
}