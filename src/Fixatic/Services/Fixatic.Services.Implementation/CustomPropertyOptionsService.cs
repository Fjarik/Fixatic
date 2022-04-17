using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class CustomPropertyOptionsService : ICustomPropertyOptionsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private CustomPropertyOptionsManager? _manager;

		public CustomPropertyOptionsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<CustomPropertyOptionsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomPropertyOption entry)
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

		public async Task<ServiceResponse<List<CustomPropertyOption>>> GetAllAsync(int propertyId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<CustomPropertyOption>>();
			try
			{
				response.Item = await _manager!.GetByPropertyAsync(propertyId);
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
				var currentCustomPropertyOption = await _currentUserService.GetUserInfoAsync();
				_manager = new CustomPropertyOptionsManager(_logger, _applicationSettings, currentCustomPropertyOption);
			}
		}
	}
}
