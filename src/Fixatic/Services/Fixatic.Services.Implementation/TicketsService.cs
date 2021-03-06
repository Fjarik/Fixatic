using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	/// <inheritdoc/>
	public class TicketsService : ITicketsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private TicketsManager? _manager;

		/// <inheritdoc/>
		public TicketsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<TicketsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public async Task<ServiceResponse<List<FullTicket>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullTicket>>();
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

		/// <inheritdoc/>
		public async Task<ServiceResponse<FullTicket?>> GetByIdAsync(int id)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<FullTicket?>();
			try
			{
				response.Item = await _manager!.GetByIdAsync(id);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<List<FullTicket>>> GetByProjectAsync(int projectId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullTicket>>();
			try
			{
				response.Item = await _manager!.GetByProjectAsync(projectId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<List<FullTicket>>> GetFollowedTicketsAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullTicket>>();
			try
			{
				response.Item = await _manager!.GetFollowedTicketsAsync();
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<List<FullTicket>>> GetAssignedTicketsAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullTicket>>();
			try
			{
				response.Item = await _manager!.GetAssignedTicketsAsync();
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public async Task<List<TicketVisibility>> GetAvailableVisiblityAsync()
		{
			await EnsureManagerAsync();

			var response = new List<TicketVisibility>();
			try
			{
				response = _manager!.GetAvailableVisiblity();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Failed to get available visibility for ticket");
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<bool>> SetAssigneeAsync(int ticketId, int userId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				response.Item = await _manager!.SetAssigneeAsync(ticketId, userId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public async Task<ServiceResponse<List<FullTicketProperty>>> GetCustomPropertiesAsync(int ticketId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullTicketProperty>>();
			try
			{
				response.Item = await _manager!.GetCustomPropertiesAsync(ticketId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<bool>> AddPropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				response.Item = await _manager!.AddPropertyOptionAsync(ticketId, propertyOptionId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}

			return response;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<bool>> RemovePropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<bool>();
			try
			{
				response.Item = await _manager!.RemovePropertyOptionAsync(ticketId, propertyOptionId);
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