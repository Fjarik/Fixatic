using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class CustomPropertiesDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public CustomPropertiesDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(CustomProperty customProperty)
		{
			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = customProperty.CustomPropertyId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO CustomProperties (description, name)
											  VALUES (@description, @name);

						SET @ID = SCOPE_IDENTITY();

						SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE CustomProperties SET description = @description, name = @name WHERE CustomProperty_ID = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
			
			cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = customProperty.Description;
			cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = customProperty.Name;

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null) id = (int)objId;
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<CustomProperty>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT CustomProperty_ID, Description, Name FROM CustomProperties;";

			var cmd = new SqlCommand(sql);

			var res = new List<CustomProperty>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new CustomProperty
					{
						CustomPropertyId = (int)r["CustomProperty_ID"],
						Description = (string)r["Description"],
						Name = (string)r["Name"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM CustomProperties WHERE CustomProperty_ID = @ID;";

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

			_logger.LogInformation($"{nameof(CustomPropertiesDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}