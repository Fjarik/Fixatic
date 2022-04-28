using System.Security.Claims;
using Fixatic.DO;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Fixatic.BO
{
	public class CurrentUserManager
	{
		private readonly ILogger _logger;
		private readonly IDBConnector _dbConnector;

		public CurrentUserManager(ILogger logger, ApplicationSettings applicationSettings)
		{
			_logger = logger;
			_dbConnector = new DBConnector(applicationSettings);
		}

		/// <summary>
		/// Login user.
		/// </summary>
		/// <param name="email">The email.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		public async Task<ClaimsPrincipal?> LoginAsync(string? email, string? password)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(LoginAsync)}...");

			if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
				return null;

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var user = await mainDo.GetUserWithPasswordAsync(email);

			if (user == null)
				return null;

			var hasher = new PasswordHasher<User>();
			var hashRes = hasher.VerifyHashedPassword(user, user.Password, password);
			if (hashRes == PasswordVerificationResult.Failed)
				return null;

			if (!user.IsEnabled)
				return null;

			var res = GenerateClaims(user);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(LoginAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Loads the user information
		/// </summary>
		/// <param name="principal">The principal.</param>
		/// <returns></returns>
		/// <exception cref="System.UnauthorizedAccessException">
		/// User not found
		/// or
		/// User is disabled
		/// </exception>
		public async Task<CurrentUser> LoadUserInfoAsync(ClaimsPrincipal? principal)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(LoadUserInfoAsync)}...");

			var email = CheckClaims(principal);

			var mainDo = new UsersDataObject(_logger, _dbConnector);

			var user = await mainDo.GetCurrentUserAsync(email);
			if (user == null)
				throw new UnauthorizedAccessException("User not found");

			if (!user.IsEnabled)
				throw new UnauthorizedAccessException("User is disabled");

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(LoadUserInfoAsync)}... Done");
			return user;
		}

		private static ClaimsPrincipal GenerateClaims(User user)
		{
			var identity = new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.Name, user.Email),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(ClaimTypes.GivenName, user.Firstname),
				new Claim(ClaimTypes.Surname, user.Lastname),
				new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString())
			}, "Fixatic");

			var res = new ClaimsPrincipal(identity);

			return res;
		}

		private static string CheckClaims(ClaimsPrincipal? principal)
		{
			if (principal == null)
				throw new UnauthorizedAccessException("User not found");

			var identity = principal.Identity;
			if (identity == null)
				throw new UnauthorizedAccessException("You are not authorized");

			if (!identity.IsAuthenticated)
				throw new UnauthorizedAccessException("You are not authorized");

			if (string.IsNullOrWhiteSpace(identity.Name))
				throw new UnauthorizedAccessException("You are not authorized");

			return identity.Name;
		}
	}
}
