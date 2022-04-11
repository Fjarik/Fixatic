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

		[Inject] private ITicketsService? TicketService { get; set; }

		[Inject] private NavigationManager? NavigationManager { get; set; }

		[Parameter] public List<Project>? Projects { get; set; }

		[Parameter] public List<Group>? Groups { get; set; }

		[Parameter] public List<ProjectCategory>? ProjectCategories { get; set; }

		[Parameter] public Group? SelectedGroup { get; set; }
		[Parameter] public ProjectCategory? SelectedCategory { get; set; }

		private List<FullTicket> _followedTickets = new();

		protected override async Task OnInitializedAsync()
		{
			var groups = await GroupsService!.GetUserGroupsAsync();

			switch (groups.IsSuccess)
			{
				case true when groups.Item != null:
					Groups = groups.Item;
					break;
				case false:
					var options = new DialogOptions { CloseOnEscapeKey = true };
					DialogService!.Show<ErrorDialog>("DB error", options);
					break;
			}

			var tickets = await TicketService!.GetAllAsync();

			switch (tickets.IsSuccess)
			{
				case true when tickets.Item != null:
					_followedTickets = new List<FullTicket>();

					// Asi není úplně nejlepší dělat DB request pro každej Ticket, ale snad to nebude vadit
					// pro naše malý množství :DDD
					foreach (var ticket in tickets.Item)
					{
						var isFollowed = await TicketsService!.IsFollowedAsync(ticket.TicketId);
						if (isFollowed.IsSuccess && isFollowed.Item)
						{
							_followedTickets.Add(ticket);
						}
					}

					StateHasChanged();

					break;
				case false:
					var options = new DialogOptions { CloseOnEscapeKey = true };
					DialogService!.Show<ErrorDialog>("DB error", options);
					break;
			}

			var categories = await CategoriesService!.GetAllAsync();

			switch (categories.IsSuccess)
			{
				case true when categories.Item != null:
					ProjectCategories = categories.Item;
					break;
				case false:
					var options = new DialogOptions { CloseOnEscapeKey = true };
					DialogService!.Show<ErrorDialog>("DB error", options);
					break;
			}
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

			NavigationManager!.NavigateTo("/project/" + item.ProjectId + "/" + item.TicketId);
		}

		private async Task CategoryFilterClickedAsync()
		{
			if (SelectedGroup != null && Projects != null && SelectedCategory != null)
			{
				await LoadProjects();

				var selectedProjects = new List<Project>();
				foreach (var p in Projects)
				{
					var ids = await ProjectsService!.GetCategoryIdsAsync(p.ProjectId);

					switch (ids.IsSuccess)
					{
						case true when ids.Item != null:
							if (ids.Item.Contains(SelectedCategory.CategoryId))
							{
								selectedProjects.Add(p);
							}

							break;
						case false:
							var optionz = new DialogOptions { CloseOnEscapeKey = true };
							DialogService!.Show<ErrorDialog>("DB error", optionz);
							break;
					}
				}

				Projects = selectedProjects;
			}
		}

		private async Task LoadProjects()
		{
			if (SelectedGroup != null)
			{
				var projects = (await ProjectsService!.GetGroupProjectsAsync(SelectedGroup.GroupId));

				switch (projects.IsSuccess)
				{
					case true when projects.Item != null:
						Projects = projects.Item.FindAll(p => p.IsEnabled);
						break;
					case false:
						var options = new DialogOptions { CloseOnEscapeKey = true };
						DialogService!.Show<ErrorDialog>("DB error", options);
						break;
				}
			}
		}
	}
}