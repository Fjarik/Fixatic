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
	public class CommentsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public CommentsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		/// <summary>
		/// Creates the or update Comment.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public async Task<int> CreateOrUpdateAsync(Comment entry)
		{
			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new CommentsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets all Comment.
		/// </summary>
		/// <returns></returns>
		public async Task<List<Comment>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new CommentsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the by ticket Comment.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>		
		public async Task<List<Comment>> GetByTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(GetByTicketAsync)}...");

			var mainDo = new CommentsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByTicketAsync(ticketId);

			if (!_currentUser.IsInternal())
			{
				res = res.FindAll(c => !c.IsInternal);
			}

			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(GetByTicketAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Deletes the Comment.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new CommentsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(CommentsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
