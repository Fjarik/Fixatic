using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class ProjectsService : IProjectsService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private ProjectsManager? _manager;

		public ProjectsService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<ProjectsService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(Project entry)
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

		public async Task<ServiceResponse<List<Project>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Project>>();
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

		public async Task<ServiceResponse<List<Project>>> GetGroupProjectsAsync(int groupId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<Project>>();
			try
			{
				response.Item = await _manager!.GetGroupProjectsAsync(groupId);
			}
			catch (Exception ex)
			{
				response.Fail(ex);
			}
			return response;
		}
		
		public async Task<ServiceResponse<List<int>>> GetCategoryIdsAsync(int projectId)
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<int>>();
			try
			{
				response.Item = await _manager!.GetCategoryIdsAsync(projectId);
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
				var currentProject = await _currentUserService.GetUserInfoAsync();
				_manager = new ProjectsManager(_logger, _applicationSettings, currentProject);
			}
		}
	}
}
