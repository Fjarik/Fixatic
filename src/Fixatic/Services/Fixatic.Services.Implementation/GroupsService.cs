using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	/// <inheritdoc/>
	public class GroupsService : IGroupsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private GroupsManager? _manager;

		public GroupsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<GroupsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		/// <inheritdoc/>
		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(Group entry)
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
		public async Task<ServiceResponse<List<Group>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Group>>();
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
		public async Task<ServiceResponse<List<Group>>> GetUserGroupsAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Group>>();
			try
			{
				response.Item = await _manager!.GetUserGroupsAsync();
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

		private async Task EnsureManagerAsync()
		{
			if (_manager == null)
			{
				var currentGroup = await _currentUserService.GetUserInfoAsync();
				_manager = new GroupsManager(_logger, _applicationSettings, currentGroup);
			}
		}
	}
}
