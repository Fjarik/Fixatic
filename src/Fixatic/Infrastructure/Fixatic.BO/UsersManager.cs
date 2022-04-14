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

		public async Task<int> CreateOrUpdateAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(CreateOrUpdateAsync)}...");

			if (user == null)
				return DB.IgnoredID;

			// TODO: Validation

			user.Email = user.Email?.ToLower();

			var hasher = new PasswordHasher<User>();

			if (!string.IsNullOrWhiteSpace(user.Password))
				user.Password = hasher.HashPassword(user, user.Password);

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(user);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<int> UpdateSansPasswordAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(UpdateSansPasswordAsync)}...");

			// TODO: Validation

			user.Email = user.Email?.ToLower();

			var hasher = new PasswordHasher<User>();

			if (!string.IsNullOrWhiteSpace(user.Password))
				user.Password = hasher.HashPassword(user, user.Password);

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.UpdateSansPasswordAsync(user);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(UpdateSansPasswordAsync)}... Done");
			return res;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<User?> GetByIdAsync(int userId)
		{
			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetByIdAsync)}...");

			var mainDo = new UsersDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByIdAsync(userId);

			_logger.LogInformation($"{nameof(UsersManager)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

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