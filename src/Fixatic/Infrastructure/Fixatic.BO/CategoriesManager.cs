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
	public class CategoriesManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public CategoriesManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<int> CreateOrUpdateAsync(ProjectCategory entry)
		{
			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new CategoriesDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<List<ProjectCategory>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new CategoriesDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new CategoriesDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(CategoriesManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
