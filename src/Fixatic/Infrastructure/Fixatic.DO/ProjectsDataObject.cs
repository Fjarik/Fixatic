using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class ProjectsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public ProjectsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Project project)
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = project.ProjectId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Projects (description, isenabled, isinternal, name, shortcut)
				VALUES (@description, @isenabled, @isinternal, @name);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Projects SET description = @description, isenabled = @isenabled, name = @name, shortcut = @shortcut WHER Project_ID = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
			// TODO(Tom) : zbytek parametrů

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null) id = (int)objId;
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Project_ID, Name, IsEnabled, IsInternal, Description FROM Projects;";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					// TODO(Tom): přidat Project objekt
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Projects WHERE Project_ID = @ID;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}