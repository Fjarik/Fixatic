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
		private readonly ApplicationSettings _applicationSettings;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public TicketsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_applicationSettings = applicationSettings;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(_applicationSettings);
		}

		/// <summary>
		/// Creates the or update Ticket.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public async Task<int> CreateOrUpdateAsync(Ticket entry)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			if (entry.TicketId != DB.IgnoredID && entry.Status == TicketStatus.Done)
			{
				//	await mainDo.FinishTicketAsync(entry.TicketId);
			}

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets all Tickets.
		/// </summary>
		/// <returns></returns>
		public async Task<List<FullTicket>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync(_currentUser.UserId);
			res = FilerInternalTickets(res);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets Tickets the by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public async Task<List<FullTicket>> GetByProjectAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByProjectAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByProjectAsync(projectId);
			res = FilerInternalTickets(res);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByProjectAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the followed tickets.
		/// </summary>
		/// <returns></returns>
		public async Task<List<FullTicket>> GetFollowedTicketsAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowedTicketsAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetFollowedTicketsAsync(_currentUser.UserId);
			res = FilerInternalTickets(res);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowedTicketsAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the assigned tickets.
		/// </summary>
		/// <returns></returns>
		public async Task<List<FullTicket>?> GetAssignedTicketsAsync()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAssignedTicketsAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAssignedTicketsAsync(_currentUser.UserId);
			res = FilerInternalTickets(res);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAssignedTicketsAsync)}... Done");
			return res;
		}

		private List<FullTicket> FilerInternalTickets(List<FullTicket> res)
		{
			if (!_currentUser.IsInternal())
			{
				res = res.FindAll(x => x.Visibility != TicketVisibility.Internal);
			}
			return res;
		}

		/// <summary>
		/// Gets the available ticket visiblity.
		/// </summary>
		/// <returns></returns>
		public List<TicketVisibility> GetAvailableVisiblity()
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAvailableVisiblity)}...");

			var res = new List<TicketVisibility>();

			res.Add(TicketVisibility.Normal);
			
			if (_currentUser.IsInternal())
			{
				res.Add(TicketVisibility.Internal);
			}

			res.Add(TicketVisibility.Public);
			
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetAvailableVisiblity)}... Done");
			return res;
		}
		

		/// <summary>
		/// Gets Ticket the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async Task<FullTicket?> GetByIdAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByIdAsync)}...");

			if (id == DB.IgnoredID)
			{
				return new FullTicket
				{
					TicketId = DB.IgnoredID,
					ProjectId = DB.IgnoredID,
					AssignedUserId = null,
					CreatorId = _currentUser.UserId,
					Content = string.Empty,
					Created = DateTime.Now,
					DateSolved = null,
					Modified = null,
					Priority = TicketPriority.Low,
					Status = TicketStatus.Created,
					Title = string.Empty,
					Type = TicketType.Bug,
					Visibility = TicketVisibility.Normal,
					Followers = 0,
					AssigneeName = string.Empty
				};
			}


			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByIdAsync(id);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the follower.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		public async Task<Follower?> GetFollowerAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowerAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetFollowerAsync(ticketId, _currentUser.UserId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetFollowerAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Sets the follow ticket.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="shouldFollow">if set to <c>true</c> [should follow].</param>
		/// <returns></returns>
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

		/// <summary>
		/// Sets the assignee.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		public async Task<bool> SetAssigneeAsync(int ticketId, int userId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(SetAssigneeAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.SetAssigneeAsync(ticketId, userId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(SetAssigneeAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the custom properties.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		public async Task<List<FullTicketProperty>> GetCustomPropertiesAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetCustomPropertiesAsync)}...");

			var propMgr = new CustomPropertiesManager(_logger, _applicationSettings, _currentUser);
			var properties = await propMgr.GetAllAsync();

			var optMgr = new CustomPropertyOptionsManager(_logger, _applicationSettings, _currentUser);
			var options = await optMgr.GetAllAsync();

			var activeIds = await GetCustomPropertyOptionIdsAsync(ticketId);

			var res = new List<FullTicketProperty>();
			foreach (var group in options.GroupBy(x => x.CustomPropertyId))
			{
				var propertyId = group.Key;
				var property = properties.FirstOrDefault(x => x.CustomPropertyId == propertyId);
				if (property == null)
					continue;

				var fullOptions = new List<FullTicketPropertyOption>();
				foreach (var option in group)
				{
					var active = activeIds.Contains(option.CustomPropertyOptionId);
					var fullOption = new FullTicketPropertyOption(option, active);
					fullOptions.Add(fullOption);
				}
				var prop = new FullTicketProperty(property, fullOptions);
				res.Add(prop);
			}

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetCustomPropertiesAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the custom property option ids.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		public async Task<List<int>> GetCustomPropertyOptionIdsAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetCustomPropertyOptionIdsAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetCustomPropertyOptionIdsAsync(ticketId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(GetCustomPropertyOptionIdsAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Adds the property option.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="propertyOptionId">The property option identifier.</param>
		/// <returns></returns>
		public async Task<bool> AddPropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(AddPropertyOptionAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.AddPropertyOptionAsync(ticketId, propertyOptionId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(AddPropertyOptionAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Removes the property option.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="propertyOptionId">The property option identifier.</param>
		/// <returns></returns>
		public async Task<bool> RemovePropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(RemovePropertyOptionAsync)}...");

			var mainDo = new TicketsDataObject(_logger, _dbConnector);
			var res = await mainDo.RemovePropertyOptionAsync(ticketId, propertyOptionId);

			_logger.LogInformation($"{nameof(TicketsManager)}.{nameof(RemovePropertyOptionAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Deletes the Ticket.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
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
