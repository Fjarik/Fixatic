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
using FixaticApp.Components;
using Fixatic.Services;
using Fixatic.Types;
using Fixatic.DO.Types;

namespace FixaticApp.Pages
{
	public partial class PropertiesPage
	{
		[Inject] private ICustomPropertiesService? CustomPropertiesService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }


		private List<CustomProperty> _cumProps = new();

		protected override async Task OnInitializedAsync()
		{
			await LoadDataAsync();
		}

		private async Task LoadDataAsync()
		{
			var res = await CustomPropertiesService!.GetAllAsync();
			if (!res.IsSuccess)
				return;

			_cumProps = res.Item!;
		}

		private async Task PropertySelectedAsync(TableRowClickEventArgs<CustomProperty> args)
		{
			if (args?.Item == null)
				return;

			await ShowPropertyDialogAsync(args.Item);
		}

		private async Task OnAddClickedAsync()
		{
			var item = new CustomProperty
			{
				CustomPropertyId = DB.IgnoredID,
				Name = string.Empty,
				Description = string.Empty,
			};
			await ShowPropertyDialogAsync(item);
		}


		private async Task ShowPropertyDialogAsync(CustomProperty item)
		{
			var parameters = new DialogParameters
			{
				{ "Property", item }
			};

			var options = new DialogOptions { CloseOnEscapeKey = true, MaxWidth = MaxWidth.Medium, FullWidth = true };
			var dialog = DialogService!.Show<CumPropDialog>("Custom property", parameters, options);

			var result = await dialog.Result;
			if (!result.Cancelled)
			{
				await LoadDataAsync();
				StateHasChanged();
			}
		}
	}
}