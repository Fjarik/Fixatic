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
	public class PublicTicketsManager
	{
		private readonly ILogger _logger;
		private readonly IDBConnector _dbConnector;

		public PublicTicketsManager(ILogger logger, ApplicationSettings applicationSettings)
		{
			_logger = logger;
			_dbConnector = new DBConnector(applicationSettings);
		}

		public async Task<List<Ticket>> GetPublicAsync()
		{
			_logger.LogInformation($"{nameof(PublicTicketsManager)}.{nameof(GetPublicAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync(TicketVisibility.Public);

			_logger.LogInformation($"{nameof(PublicTicketsManager)}.{nameof(GetPublicAsync)}... Done");
			return res;
		}

	}
}
