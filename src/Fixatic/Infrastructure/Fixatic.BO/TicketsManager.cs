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
	public class TicketsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public TicketsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<int> CreateOrUpdateAsync(Ticket entry)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<List<Ticket>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
