using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	/// <inheritdoc/>
	public class CategoriesService : ICategoriesService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private CategoriesManager? _manager;

		public CategoriesService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<CategoriesService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(ProjectCategory entry)
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
		public async Task<ServiceResponse<List<ProjectCategory>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<ProjectCategory>>();
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
				var currentCategory = await _currentUserService.GetUserInfoAsync();
				_manager = new CategoriesManager(_logger, _applicationSettings, currentCategory);
			}
		}
	}
}
