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
using Fixatic.Services;

namespace FixaticApp.Pages.Public
{
	public partial class Index
	{
		// !!! this page is for users that are NOT logged in !!!

		[Inject]
		private ICurrentUserService CurrentUserService { get; set; }

		[Inject]
		private NavigationManager NavigationManager { get; set; }

		private bool _load = false;

		protected override async Task OnInitializedAsync()
		{
			var isLoggedIn = await CurrentUserService.IsLoggedInAsync();
			if (isLoggedIn)
			{
				NavigationManager.NavigateTo("/dashboard");
				return;
			}
			_load = true;
		}

	}
}