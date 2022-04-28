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
	public class CustomPropertyOptionsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public CustomPropertyOptionsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		/// <summary>
		/// Creates the or update CustomPropertyOption.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public async Task<int> CreateOrUpdateAsync(CustomPropertyOption entry)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new CustomPropertyOptionsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets all CustomPropertyOptions.
		/// </summary>
		/// <param name="propertyId">The property identifier.</param>
		/// <returns></returns>
		public async Task<List<CustomPropertyOption>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new CustomPropertyOptionsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(GetAllAsync)}... Done");
			return res.OrderBy(x => x.Sequence).ToList();
		}

		/// <summary>
		/// Gets CustomPropertyOptions by property.
		/// </summary>
		/// <param name="propertyId">The property identifier.</param>
		/// <returns></returns>
		public async Task<List<CustomPropertyOption>> GetByPropertyAsync(int propertyId)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(GetByPropertyAsync)}...");

			var mainDo = new CustomPropertyOptionsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByPropertyAsync(propertyId);

			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(GetByPropertyAsync)}... Done");
			return res.OrderBy(x => x.Sequence).ToList();
		}

		/// <summary>
		/// Deletes the CustomPropertyOption.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new CustomPropertyOptionsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(CustomPropertyOptionsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
