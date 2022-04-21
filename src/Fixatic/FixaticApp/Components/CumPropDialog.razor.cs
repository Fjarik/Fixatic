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
using Fixatic.DO.Types;

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
		private bool MainFormValid => !string.IsNullOrWhiteSpace(Property?.Name);
		private bool OptionFormValid => !string.IsNullOrWhiteSpace(_optionValue);

		private List<CustomPropertyOption> _options = new();
		private int _prevId = 0;

		private string _optionValue = string.Empty;

		protected override async Task OnParametersSetAsync()
		{
			if (Property == null)
				return;

			if (Property.CustomPropertyId == _prevId)
				return;

			_prevId = Property.CustomPropertyId;
			await LoadOptions();
		}

		private async Task LoadOptions()
		{
			if (Property == null)
				return;

			var res = await CustomPropertyOptionsService!.GetAllAsync(Property.CustomPropertyId);
			if (res.IsSuccess)
			{
				_options = res.Item!;
			}
		}

		private async Task Submit()
		{
			if (!MainFormValid || Property == null)
				return;

			await CustomPropertiesService!.CreateOrUpdateAsync(Property);

			MudDialog!.Close(DialogResult.Ok(string.Empty));
		}

		private async Task DeleteOption(CustomPropertyOption option)
		{
			if (!option.CanDelete)
				return;

			await CustomPropertyOptionsService!.DeleteAsync(option.CustomPropertyOptionId);
			_options.Remove(option);
		}

		private async Task AddOption()
		{
			if (!OptionFormValid || string.IsNullOrWhiteSpace(_optionValue))
				return;

			var newSeq = _options.Count > 0 ? _options.Max(x => x.Sequence) + 1 : 1;
			var opt = new CustomPropertyOption
			{
				CustomPropertyOptionId = DB.IgnoredID,
				CustomPropertyId = Property!.CustomPropertyId,
				Content = _optionValue,
				IsEnabled = true,
				Sequence = newSeq
			};
			await CustomPropertyOptionsService!.CreateOrUpdateAsync(opt);
			_optionValue = string.Empty;
			await LoadOptions();
		}

		private void Cancel()
		{
			MudDialog!.Cancel();
		}

	}
}