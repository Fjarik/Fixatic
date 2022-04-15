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
using Fixatic.Types;
using Fixatic.Services;

namespace FixaticApp.Components
{
	public partial class CumPropDialog
	{
		[CascadingParameter]
		private MudDialogInstance? MudDialog { get; set; }

		[Parameter]
		public CustomProperty? Property { get; set; }

		[Inject]
		private ICustomPropertiesService? CustomPropertiesService { get; set; }

		[Inject]
		private ICustomPropertyOptionsService? CustomPropertyOptionsService { get; set; }

		private bool IsCreate => Property?.CustomPropertyId > 0;

		private List<CustomPropertyOption> _options = new();
		private int _prevId = 0;
		private bool _formValid;

		protected override async Task OnParametersSetAsync()
		{
			if (Property == null)
				return;

			if (Property.CustomPropertyId == _prevId)
				return;

			_prevId = Property.CustomPropertyId;
			var res = await CustomPropertyOptionsService!.GetAllAsync(Property.CustomPropertyId);
			if (res.IsSuccess)
			{
				_options = res.Item!;
			}

		}

		private async Task Submit()
		{
			if (!_formValid)
				return;

			var res = await CustomPropertiesService!.CreateOrUpdateAsync(Property!);

			MudDialog!.Close(DialogResult.Ok(string.Empty));
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

	}
}