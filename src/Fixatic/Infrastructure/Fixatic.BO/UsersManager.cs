using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.DO;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Fixatic.BO
{
	public class UsersManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public UsersManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		/// <summary>
		/// Creates the or update User.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public async Task<int> CreateOrUpdateAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(CreateOrUpdateAsync)}...");

			if (user == null)
				return DB.IgnoredID;

			user.Email = user.Email?.ToLower();

			var hasher = new PasswordHasher<User>();

			if (!string.IsNullOrWhiteSpace(user.Password))
				user.Password = hasher.HashPassword(user, user.Password);

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(user);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Updates the sans password.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>		
		public async Task<int> UpdateSansPasswordAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(UpdateSansPasswordAsync)}...");

			user.Email = user.Email?.ToLower();

			var hasher = new PasswordHasher<User>();

			if (!string.IsNullOrWhiteSpace(user.Password))
				user.Password = hasher.HashPassword(user, user.Password);

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.UpdateSansPasswordAsync(user);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(UpdateSansPasswordAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets all Users.
		/// </summary>
		/// <returns></returns>
		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the possible ticket assignees.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		public async Task<List<BasicUserInfo>> GetPossibleTicketAssigneesAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetPossibleTicketAssigneesAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.GetPossibleTicketAssigneesAsync(ticketId);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetPossibleTicketAssigneesAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets User the by identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>		
		public async Task<User?> GetByIdAsync(int userId)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetByIdAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByIdAsync(userId);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Deletes the User.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}