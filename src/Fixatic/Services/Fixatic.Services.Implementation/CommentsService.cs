using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class CommentsService : ICommentsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private CommentsManager? _manager;

		public CommentsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<CommentsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(Comment entry)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<int>();
			try
			{
				response.Item = await _manager!.CreateOrUpdateAsync(entry);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		public async Task<ServiceResponse<List<Comment>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Comment>>();
			try
			{
				response.Item = await _manager!.GetAllAsync();
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}


		public async Task<ServiceResponse<List<Comment>>> GetByTicketAsync(int ticketId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Comment>>();
			try
			{
				response.Item = await _manager!.GetByTicketAsync(ticketId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		public async Task<ServiceResponse<List<Comment>>> GetByTicketUserVisibleAsync(int ticketId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Comment>>();
			try
			{
				response.Item = await _manager!.GetByTicketAsync(ticketId);

				var currentUser = await _currentUserService.GetUserInfoAsync();
				if (!currentUser.IsInternal())
				{
					response.Item = response.Item.FindAll(c => !c.IsInternal);
				}
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		public async Task<ServiceResponse<bool>> DeleteAsync(int id)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				response.Item = await _manager!.DeleteAsync(id);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		private async Task EnsureManagerAsync()
		{
			if (_manager == null)
			{
				var currentComment = await _currentUserService.GetUserInfoAsync();
				_manager = new CommentsManager(_logger, _applicationSettings, currentComment);
			}
		}
	}
}