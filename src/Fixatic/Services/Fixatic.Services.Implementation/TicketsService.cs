using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class TicketsService : ITicketsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private TicketsManager? _manager;

		public TicketsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<TicketsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(Ticket entry)
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

		public async Task<ServiceResponse<List<Ticket>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Ticket>>();
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

		public async Task<ServiceResponse<bool>> IsFollowedAsync(int ticketId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				var follower = await _manager!.GetFollowerAsync(ticketId);
				response.Item = follower != null;
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}
			return response;
		}

		public async Task<ServiceResponse<bool>> SetFollowTicketAsync(int ticketId, bool shouldFollow)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				response.Item = await _manager!.SetFollowTicketAsync(ticketId, shouldFollow);
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
				var currentUser = await _currentUserService.GetUserInfoAsync();
				_manager = new TicketsManager(_logger, _applicationSettings, currentUser);
			}
		}

	}
}
