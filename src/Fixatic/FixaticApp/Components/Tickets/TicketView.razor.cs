using Fixatic.Services;
using Fixatic.Types;
using Microsoft.AspNetCore.Components;

namespace FixaticApp.Components.Tickets
{
	public partial class TicketView
	{
		[Parameter] public FullTicket? Model { get; set; }

		[Inject] private ICommentsService? CommentsService { get; set; }

		[Inject] private ICustomPropertiesService? CustomPropertiesService { get; set; }

		[Inject] private ICurrentUserService? CurrentUserService { get; set; }

		private List<Comment> Comments { get; set; } = new();
		private List<FullProperty> Properties { get; set; } = new();

		private int PrevId { get; set; } = -1;

		protected override async Task OnParametersSetAsync()
		{
			if (Model == null)
			{
				PrevId = -1;
				return;
			}

			if (PrevId == Model.TicketId)
				return;

			PrevId = Model.TicketId;

			await LoadInfoAsync();
		}

		private async Task LoadInfoAsync()
		{
			var commentsRes = await CommentsService!.GetByTicketUserVisibleAsync(Model!.TicketId);
			if (commentsRes.IsSuccess)
			{
				Comments = commentsRes.Item ?? new List<Comment>();
			}

			var propsRes = await CustomPropertiesService!.GetByTicketAsync(Model.TicketId);
			if (propsRes.IsSuccess)
			{
				Properties = propsRes.Item ?? new List<FullProperty>();
			}
		}

		private string GetAssigneeName()
		{
			if (string.IsNullOrWhiteSpace(Model?.AssigneeName))
			{
				return "None";
			}

			return Model.AssigneeName;
		}

		public string GetFollowersText()
		{
			if (Model == null)
				return "0 followers";

			if (Model.Followers > 1 || Model.Followers < 1)
				return $"{Model.Followers} followers";

			return "1 follower";
		}
	}
}