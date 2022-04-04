﻿using Fixatic.BO;
using Fixatic.Services.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class UsersService : IUsersService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly ICurrentUserService _currentUserService;
		private UsersManager? _manager;

		public UsersService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<UsersService> logger,
			ICurrentUserService currentUserService)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_currentUserService = currentUserService;
		}

		public async Task<ServiceResponse<int>> CreateOrUpdateAsync(User entry)
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

		public async Task<ServiceResponse<List<User>>> GetAllAsync()
		{
			await EnsureManagerAsync();

			var response = new ServiceResponse<List<User>>();
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
				_manager = new UsersManager(_logger, _applicationSettings, currentUser);
			}
		}
	}
}