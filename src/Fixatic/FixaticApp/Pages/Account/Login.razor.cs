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
using System.ComponentModel.DataAnnotations;
using FixaticApp.Types;
using Microsoft.AspNetCore.WebUtilities;

namespace FixaticApp.Pages.Account
{
	public partial class Login
	{
		[Inject]
		protected NavigationManager? NavigationManager { get; set; }

		private string _message = string.Empty;

		protected override void OnInitialized()
		{
			var uri = NavigationManager!.ToAbsoluteUri(NavigationManager.Uri);
			if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("msg", out var msg))
			{
				_message = msg.ToString();
			}
		}
	}
}