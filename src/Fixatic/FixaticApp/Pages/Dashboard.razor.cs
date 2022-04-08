using Fixatic.Services;
using Fixatic.Services.Implementation;
using Fixatic.Types;
using FixaticApp.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.VisualBasic.CompilerServices;
using MudBlazor;

namespace FixaticApp.Pages
{
	public partial class Dashboard
	{
		[Parameter] public List<Project>? Projects { get; set; }

		[Parameter] public List<Group>? Groups { get; set; }

		[Parameter] public Group? SelectedGroup { get; set; }

		[Inject] private ITicketsService TicketsService { get; set; }

		private MudTable<Ticket> ticketsTable;
		private List<Ticket>? FollowedTickets { get; set; }

		protected override async Task OnInitializedAsync()
		{
			var groups = await _groupsService.GetUserGroupsAsync();

			switch (groups.IsSuccess)
			{
				case true when groups.Item != null:
					Groups = groups.Item;
					break;
				case false:
					var options = new DialogOptions {CloseOnEscapeKey = true};
					_dialogService.Show<ErrorDialog>("DB error", options);
					break;
			}

			var tickets = await _ticketService.GetAllAsync();

			switch (tickets.IsSuccess)
			{
				case true when tickets.Item != null:
					FollowedTickets = new List<Ticket>();

					// asi není úplně nejlepší dělat DB request pro každej Ticket, ale snad to nebude vadit
					// pro naše malý množství :DDD
					foreach (var ticket in tickets.Item)
					{
						var isFollowed = await TicketsService.IsFollowedAsync(ticket.TicketId);
						if (isFollowed.IsSuccess && isFollowed.Item)
						{
							FollowedTickets.Add(ticket);
						}
					}
					
					StateHasChanged();

					break;
				case false:
					var options = new DialogOptions {CloseOnEscapeKey = true};
					_dialogService.Show<ErrorDialog>("DB error", options);
					break;
			}
		}

		private async Task GroupButtonClicked(Group group)
		{
			this.SelectedGroup = group;
			var projects = (await _projectsService.GetGroupProjectsAsync(group.GroupId));

			switch (projects.IsSuccess)
			{
				case true when projects.Item != null:
					Projects = projects.Item.FindAll(p => p.IsEnabled);
					break;
				case false:
					var options = new DialogOptions {CloseOnEscapeKey = true};
					_dialogService.Show<ErrorDialog>("DB error", options);
					break;
			}

			StateHasChanged();
		}

		private void ReturnButtonClicked()
		{
			SelectedGroup = null;
			Projects = null;
		}

		private void ProjectButtonClicked(Project project)
		{
			_navigationManager.NavigateTo("/project/" + project.ProjectId);
		}
	}
}