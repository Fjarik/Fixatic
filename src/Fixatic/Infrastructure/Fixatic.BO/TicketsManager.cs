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

		public async Task<List<FullTicket>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync(_currentUser.UserId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<List<FullTicket>> GetByProjectAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByProjectAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByProjectAsync(projectId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByProjectAsync)}... Done");
			return res;
		}

		public async Task<List<FullTicket>> GetFollowedTicketsAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowedTicketsAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetFollowedTicketsAsync(_currentUser.UserId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowedTicketsAsync)}... Done");
			return res;
		}

		public async Task<FullTicket?> GetByIdAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByIdAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByIdAsync(id);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		public async Task<Follower?> GetFollowerAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowerAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetFollowerAsync(ticketId, _currentUser.UserId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowerAsync)}... Done");
			return res;
		}

		public async Task<bool> SetFollowTicketAsync(int ticketId, bool shouldFollow)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(SetFollowTicketAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			bool success;
			if (shouldFollow)
			{
				success = await mainDo.AddFollowerAsync(ticketId, _currentUser.UserId, 0);
			}
			else
			{
				success = await mainDo.RemoveFollowerAsync(ticketId, _currentUser.UserId);
			}

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(SetFollowTicketAsync)}... Done");
			return success && shouldFollow;
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
