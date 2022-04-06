using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.DO;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.BO
{
	public class GroupsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public GroupsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<int> CreateOrUpdateAsync(Group entry)
		{
			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new GroupsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<List<Group>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new GroupsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}
		
		public async Task<List<Group>> GetUserGroupsAsync()
		{
			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(GetUserGroupsAsync)}...");

			var mainDo = new GroupsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetUserGroups(_currentUser.UserId);

			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(GetUserGroupsAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new GroupsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(GroupsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
