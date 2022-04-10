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
	public class AttachementsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public AttachementsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<int> CreateOrUpdateAsync(Attachement entry)
		{
			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new AttachementsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		public async Task<List<Attachement>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new AttachementsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<List<Attachement>> GetByTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(GetByTicketAsync)}...");

			var mainDo = new AttachementsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByTicketAsync(ticketId);

			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(GetByTicketAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new AttachementsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(AttachementsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}

	}
}
