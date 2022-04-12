using Fixatic.Services;
using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace FixaticApp.Pages
{
	public partial class Dashboard
	{
		[Inject] private ITicketsService? TicketsService { get; set; }

		[Inject] private ICategoriesService? CategoriesService { get; set; }

		[Inject] private IProjectsService? ProjectsService { get; set; }

		[Inject] private IGroupsService? GroupsService { get; set; }

		[Inject] private IDialogService? DialogService { get; set; }

		[Inject] private NavigationManager? NavigationManager { get; set; }

		[Parameter] public List<Project>? Projects { get; set; }

		[Parameter] public List<Group>? Groups { get; set; }

		[Parameter] public List<ProjectCategory>? ProjectCategories { get; set; }

		[Parameter] public Group? SelectedGroup { get; set; }
		[Parameter] public ProjectCategory? SelectedCategory { get; set; }

		private List<FullTicket> _followedTickets = new();

		protected override async Task OnInitializedAsync()
		{
			var groupsRes = await GroupsService!.GetUserGroupsAsync();
			if (!groupsRes.IsSuccess)
			{
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService!.Show<ErrorDialog>("DB error", options);
				return;
			}

			Groups = groupsRes.Item!;

			var ticketsRes = await TicketsService!.GetFollowedTicketsAsync();
			if (!ticketsRes.IsSuccess)
			{
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService!.Show<ErrorDialog>("DB error", options);
				return;
			}
			_followedTickets = ticketsRes.Item!;

			var categoriesRes = await CategoriesService!.GetAllAsync();
			if (!categoriesRes.IsSuccess)
			{
				var options = new DialogOptions { CloseOnEscapeKey = true };
				DialogService!.Show<ErrorDialog>("DB error", options);
				return;
			}
			ProjectCategories = categoriesRes.Item!;

		}

		private async Task GroupButtonClicked(Group group)
		{
			this.SelectedGroup = group;
			await LoadProjects();

			StateHasChanged();
		}

		private void ReturnButtonClicked()
		{
			SelectedGroup = null;
			Projects = null;
		}

		private void ProjectButtonClicked(Project project)
		{
			NavigationManager!.NavigateTo("/project/" + project.ProjectId);
		}

		private void OnTicketClicked(TableRowClickEventArgs<FullTicket> args)
		{
			if (args == null || args.Item == null)
			{
				return;
			}
			var item = args.Item;

			NavigationManager!.NavigateTo("/tickets/" + item.TicketId);
		}

		private async Task CategoryFilterClickedAsync()
		{
			if (SelectedGroup != null && Projects != null && SelectedCategory != null)
			{
				await LoadProjects();

				var selectedProjects = new List<Project>();
				foreach (var p in Projects)
				{
					var idsRes = await ProjectsService!.GetCategoryIdsAsync(p.ProjectId);
					if (!idsRes.IsSuccess)
					{
						var options = new DialogOptions { CloseOnEscapeKey = true };
						DialogService!.Show<ErrorDialog>("DB error", options);
						return;
					}

					if (idsRes.Item!.Contains(SelectedCategory.CategoryId))
					{
						selectedProjects.Add(p);
					}

				}

				Projects = selectedProjects;
			}
		}

		private async Task LoadProjects()
		{
			if (SelectedGroup != null)
			{
				var projectRes = (await ProjectsService!.GetGroupProjectsAsync(SelectedGroup.GroupId));
				if (!projectRes.IsSuccess)
				{
					var options = new DialogOptions { CloseOnEscapeKey = true };
					DialogService!.Show<ErrorDialog>("DB error", options);
					return;
				}
				Projects = projectRes.Item!.FindAll(p => p.IsEnabled);
			}
		}
	}
}