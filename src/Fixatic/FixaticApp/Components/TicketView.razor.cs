using Fixatic.Services;
using Fixatic.Types;
using Microsoft.AspNetCore.Components;

namespace FixaticApp.Components
{
	public partial class TicketView
	{
		[Parameter] public FullTicket? Model { get; set; }
		
		[Inject]
		private ICommentsService? CommentsService { get; set; }
		
		[Inject]
		private IAttachementsService? AttachementsService { get; set; }

		private List<Comment> Comments { get; set; } = new();
		private List<Attachement> Attachements { get; set; } = new();

		private int PrevId { get; set; } = -1;

		private string GetAssigneeName()
		{
			if (string.IsNullOrWhiteSpace(Model?.AssigneeName))
			{
				return "None";
			}

			return Model.AssigneeName;
		}

		protected override async Task OnParametersSetAsync()
		{
			if (Model == null)
				return;

			if (PrevId == Model.TicketId)
				return;

			PrevId = Model.TicketId;

			await LoadInfoAsync();
		}

		private async Task LoadInfoAsync()
		{
			var commentsRes = await CommentsService!.GetByTicketAsync(Model!.TicketId);
			if (commentsRes.IsSuccess)
			{
				Comments = commentsRes.Item ?? new List<Comment>();
			}

			var attachementsRes = await AttachementsService!.GetByTicketAsync(Model.TicketId);
			if (attachementsRes.IsSuccess)
			{
				Attachements = attachementsRes.Item ?? new List<Attachement>();
			}

		}

	}
}