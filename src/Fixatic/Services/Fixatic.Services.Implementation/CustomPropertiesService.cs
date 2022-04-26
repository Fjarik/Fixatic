using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	/// <inheritdoc/>
	public class CustomPropertiesService : ICustomPropertiesService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private CustomPropertiesManager? _manager;

		public CustomPropertiesService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<CustomPropertiesService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomProperty entry)
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
		public async Task<ServiceResponse<List<CustomProperty>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<CustomProperty>>();
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
		public async Task<ServiceResponse<List<FullProperty>>> GetByTicketAsync(int ticketId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<FullProperty>>();
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
		private async Task EnsureManagerAsync()
		{
			if (_manager == null)
			{
				var currentCustomProperty = await _currentUserService.GetUserInfoAsync();
				_manager = new CustomPropertiesManager(_logger, _applicationSettings, currentCustomProperty);
			}
		}

	}
}
