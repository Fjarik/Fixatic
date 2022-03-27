using System.Data;
using System.Data.SqlClient;
using System.Text;
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
				VALUES (@description, @isenabled, @isinternal, @name, @shortcut);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Projects SET description = @description, isenabled = @isenabled, isinternal = @isinternal, name = @name, shortcut = @shortcut WHERE Project_ID = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = project.Description;
			cmd.Parameters.Add("@isenabled", SqlDbType.Bit).Value = project.IsEnabled;
			cmd.Parameters.Add("@isinternal", SqlDbType.Bit).Value = project.IsInternal;
			cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = project.Name;
			cmd.Parameters.Add("@shortcut", SqlDbType.NVarChar).Value = project.Shortcut;

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

		public async Task<List<Project>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Project_ID, Name, IsEnabled, IsInternal, Description, Shortcut FROM Projects;";

			var cmd = new SqlCommand(sql);

			var res = new List<Project>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Project
					{
						ProjectId = (int)r["Project_ID"],
						Description = (string)r["Description"],
						IsEnabled = (bool)r["IsEnabled"],
						IsInternal = (bool)r["IsInternal"],
						Name = (string)r["Name"],
						Shortcut = (string)r["Shortcut"]
					});
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

			var sql = @"
				DELETE FROM ProjectsAccess WHERE Project_ID = @ID;
				DELETE FROM ProjectsCategories WHERE Project_ID = @ID;
				DELETE FROM Projects WHERE Project_ID = @ID;";

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

		public async Task<bool> SetCategoriesAsync(int projectId, List<int> categoryIds)
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(SetCategoriesAsync)}...");

			var sb = new StringBuilder();

			sb.Append(@"DELETE FROM ProjectsCategories WHERE Project_ID = @ID;");

			if (categoryIds.Any())
			{
				sb.Append(@"INSERT INTO ProjectsCategories (category_id, project_id) VALUES ");
				for (var i = 0; i < categoryIds.Count; i++)
				{
					sb.Append($@"(@category_id{i}, @ID)");
					sb.Append(i == categoryIds.Count - 1 ? ";" : ",");
				}
			}

			var cmd = new SqlCommand(sb.ToString());
			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = projectId;
			for (var i = 0; i < categoryIds.Count; i++)
			{
				cmd.Parameters.Add($"@category_id{i}", SqlDbType.Int).Value = categoryIds[i];
			}

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(SetCategoriesAsync)}... Done");
			return true;
		}

		public async Task<List<int>> GetCategoryIdsAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(GetCategoryIdsAsync)}...");

			var sql = @"SELECT Category_ID FROM ProjectsCategories WHERE Project_ID = @project_id;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = projectId;

			var res = new List<int>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add((int)r["Category_ID"]);
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectsDataObject)}.{nameof(GetCategoryIdsAsync)}... Done");
			return res;
		}
	}
}