using System.Data;
using System.Data.SqlClient;
using System.Text;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class CategoriesDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public CategoriesDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(ProjectCategory category)
		{
			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = category.CategoryId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Categories (description, name)
				VALUES (@description, @name);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Categories SET description = @description, name = @name WHER Category_ID = @ID;";
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

			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<ProjectCategory>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Category_ID, Name, Description FROM Categories;";

			var cmd = new SqlCommand(sql);

			var res = new List<ProjectCategory>();

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

			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Categories WHERE Category_ID = @ID;";

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

			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
		public async Task<List<int>> GetProjectIdsAsync(int categoryId)
		{
			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(GetProjectIdsAsync)}...");

			var sql = @"SELECT Project_ID FROM ProjectCategories WHERE Category_ID = @categoryId;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@categoryId", SqlDbType.Int).Value = categoryId;

			var res = new List<int>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add((int)r["Project_ID"]);
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CategoriesDataObject)}.{nameof(GetProjectIdsAsync)}... Done");
			return res;
		}
	}
}