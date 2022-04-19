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
using Fixatic.Types;
using Fixatic.Types.Extensions;
using FixaticApp.Components;
using FixaticApp.Types;
using Fixatic.Services;

namespace FixaticApp.Shared
{
	public partial class AppLayout
	{
		[CascadingParameter(Name = "DarkMode")]
		public bool DarkMode { get; set; }

		[CascadingParameter(Name = "ToggleDarkMode")]
		public Action ToggleDarkMode { get; set; }

		[Inject]
		private ICurrentUserService? CurrentUserService { get; set; }

		private CurrentUser? _currentUser;

		private bool _drawerOpen = true;

		protected override async Task OnInitializedAsync()
		{
			_currentUser = await CurrentUserService!.GetUserInfoAsync();
			await base.OnInitializedAsync();
		}

		private void DrawerToggle()
		{
			_drawerOpen = !_drawerOpen;
		}
	}
}