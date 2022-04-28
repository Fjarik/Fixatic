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
	public class ProjectsManager
	{
		private readonly ILogger _logger;
		private readonly CurrentUser _currentUser;
		private readonly IDBConnector _dbConnector;

		public ProjectsManager(ILogger logger, ApplicationSettings applicationSettings, CurrentUser currentUser)
		{
			_logger = logger;
			_currentUser = currentUser;
			_dbConnector = new DBConnector(applicationSettings);
		}

		/// <summary>
		/// Creates the or update Project.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		public async Task<int> CreateOrUpdateAsync(Project entry)
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(CreateOrUpdateAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.CreateOrUpdateAsync(entry);

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(CreateOrUpdateAsync)}... Done");
			return res;
		}


		/// <summary>
		/// Gets all ProjectsProject.
		/// </summary>
		/// <returns></returns>
		public async Task<List<Project>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetAllAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetAllAsync();

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the group projects.
		/// </summary>
		/// <param name="groupGroupId">The group group identifier.</param>
		/// <returns></returns>
		public async Task<List<Project>?> GetGroupProjectsAsync(int groupId)
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetGroupProjectsAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetGroupProjectsAsync(groupId);

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetGroupProjectsAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets Project the by identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public async Task<Project?> GetByIdAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetByIdAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetByIdAsync(projectId);

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Gets the category ids.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		public async Task<List<int>?> GetCategoryIdsAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetCategoryIdsAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.GetCategoryIdsAsync(projectId);

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(GetCategoryIdsAsync)}... Done");
			return res;
		}

		/// <summary>
		/// Deletes the Project.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(DeleteAsync)}...");

			var mainDo = new ProjectsDataObject(_logger, _dbConnector);
			var res = await mainDo.DeleteAsync(id);

			_logger.LogInformation($"{nameof(ProjectsManager)}.{nameof(DeleteAsync)}... Done");
			return res;
		}
	}
}
