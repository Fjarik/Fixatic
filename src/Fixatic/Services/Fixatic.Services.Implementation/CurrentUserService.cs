using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Fixatic.BO;
using Fixatic.Types;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fixatic.Services.Implementation
{
	public class CurrentUserService : ICurrentUserService
	{
		private readonly ApplicationSettings _applicationSettings;
		private readonly ILogger _logger;
		private readonly AuthenticationStateProvider _authenticationStateProvider;
		private readonly CurrentUserManager _manager;
		
		private CurrentUser? _currentUser;
		private ClaimsPrincipal? _principal;

		private DateTimeOffset _userLoadedDateTimeOffset = DateTimeOffset.MinValue;
		private const int CacheSeconds = 300;

		private double CacheAge => (DateTimeOffset.UtcNow - _userLoadedDateTimeOffset).TotalSeconds;
		private bool IsExpired => CacheAge > CacheSeconds;
		private bool ShouldReload => _currentUser == null || IsExpired;

		public CurrentUserService(
			IOptions<ApplicationSettings> applicationSettings,
			ILogger<UsersService> logger,
			AuthenticationStateProvider authenticationStateProvider)
		{
			_applicationSettings = applicationSettings.Value;
			_logger = logger;
			_authenticationStateProvider = authenticationStateProvider;
			_manager = new CurrentUserManager(_logger, _applicationSettings);
		}


		public async Task<CurrentUser> GetUserInfoAsync()
		{
			await EnsureCurrentUserAsync();

			return _currentUser!;
		}

		private async Task EnsureCurrentUserAsync()
		{
			if (ShouldReload)
				await LoadCurrentUserAsync();
		}

		private async Task LoadCurrentUserAsync()
		{
			var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
			_principal = authState?.User;
			_currentUser = await _manager.LoadUserInfoAsync(_principal);
			_userLoadedDateTimeOffset = DateTimeOffset.UtcNow;
		}

		public void InvalidateCache()
		{
			_currentUser = null;
		}
	}
}
