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

		public async Task<List<FullProperty>> GetByTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(GetByTicketAsync)}...");

			var mainDo = new CustomPropertiesDataObject(_logger, _dbConnector);
			var rows = await mainDo.GetByTicketAsync(ticketId);

			var res = new List<FullProperty>();
			foreach (var group in rows.GroupBy(x => x.CustomPropertyId))
			{
				var first = group.FirstOrDefault();
				if (first == null)
					continue;

				var prop = new FullProperty
				{
					CustomPropertyId = group.Key,
					Name = first.Name,
					Description = first.Description,
					Options = new List<CustomPropertyOption>(),
				};

				foreach (var option in group)
				{
					prop.Options.Add(new CustomPropertyOption
					{
						CustomPropertyId = prop.CustomPropertyId,
						CustomPropertyOptionId = option.CustomPropertyOptionId,
						Content = option.Content,
						IsEnabled = option.IsEnabled,
						Sequence = option.Sequence,
					});
				}
				
				res.Add(prop);
			}

			_logger.LogInformation($"{nameof(CustomPropertiesManager)}.{nameof(GetByTicketAsync)}... Done");
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
