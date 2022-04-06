using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Pages
{
	public partial class Dashboard
	{
		[Parameter] public List<Project>? Projects { get; set; }
		
		[Parameter] public List<Group>? Groups { get; set; }

		[Parameter] public Group? SelectedGroup { get; set; }
		
		// TODO: just for testing, remove later
		[Parameter] public Ticket? SelectedTicket { get; set; }
		
		protected override async Task OnInitializedAsync()
		{
			var groups = await _groupsService.GetUserGroupsAsync();

			switch (groups.IsSuccess)
			{
				case true when groups.Item != null:
					Groups = groups.Item;
					break;
				case false:
					var options = new DialogOptions { CloseOnEscapeKey = true };
					_dialogService.Show<ErrorDialog>("DB error", options);
					break;
			}
		}

		private async void GroupButtonClicked(Group group)
		{
			this.SelectedGroup = group;
			var projects = (await _projectsService.GetGroupProjectsAsync(group.GroupId));
			
			switch (projects.IsSuccess)
			{
				case true when projects.Item != null:
					Projects = projects.Item.FindAll(p => p.IsEnabled);
					break;
				case false:
					var options = new DialogOptions { CloseOnEscapeKey = true };
					_dialogService.Show<ErrorDialog>("DB error", options);
					break;
			}
		}

		private void ReturnButtonClicked()
		{
			SelectedGroup = null;
			// TODO: FIXME z nějakého důvodu mi to neukazuje žádné projekty když tohle není zakomentované ?!
			// A vždycky to musím refreshnout, aby se mi zobrazily ty správný.. wtf :DD
			// Projects = null;
		}

		private async void ProjectButtonClicked()
		{
			// TODO: navigovat na projekt ... jak to vyřešit v UI ?
		}
	}
}