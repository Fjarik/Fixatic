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
	public class CustomPropertiesManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public CustomPropertiesManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<int> CreateOrUpdateAsync(CustomProperty entry)
		{
			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new CustomPropertiesDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<List<CustomProperty>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new CustomPropertiesDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new CustomPropertiesDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
