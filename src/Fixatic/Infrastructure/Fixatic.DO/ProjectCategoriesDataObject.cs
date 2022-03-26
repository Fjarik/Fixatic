using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class ProjectCategoriesDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public ProjectCategoriesDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(ProjectCategory projectCategory)
		{
			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = projectCategory.CategoryId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO ProjectCategories (category_id, project_id)
				VALUES (@category_id, @project_id);";
			}
			else
			{
				//TODO: Vyřešit ID
				sql = @"UPDATE ProjectCategories SET category_id = @category_id, project_id = @project_id;";
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

			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Category_ID, Project_ID FROM ProjectCategories;";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					// TODO(Tom): přidat ProjectCategory objekt
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM ProjectCategories;";

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

			_logger.LogInformation($"{nameof(ProjectCategoriesDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}