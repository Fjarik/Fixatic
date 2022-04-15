using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class CustomPropertyOptionsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public CustomPropertyOptionsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(CustomPropertyOption customPropertyOption)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = customPropertyOption.CustomPropertyOptionId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO CustomPropertyOptions (content, isenabled, sequence, customproperty_id)
				VALUES (@content, @isenabled, @sequence, @customproperty_id);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE CustomPropertyOptions SET content = @content, isenabled = @isenabled, sequence = @sequence, customproperty_id = @customproperty_id 
				WHERE CustomPropertyOption_ID = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@content", SqlDbType.NText).Value = customPropertyOption.Content;
			cmd.Parameters.Add("@isenabled", SqlDbType.Bit).Value = customPropertyOption.IsEnabled;
			cmd.Parameters.Add("@sequence", SqlDbType.Int).Value = customPropertyOption.Sequence;
			cmd.Parameters.Add("@customproperty_id", SqlDbType.Int).Value = customPropertyOption.CustomPropertyId;

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null) id = (int)objId;
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<CustomPropertyOption>> GetAllAsync(int propertyId)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT 
							CustomPropertyOption_ID, 
							Content,
							IsEnabled, 
							Sequence, 
							CustomProperty_ID 
						FROM CustomPropertyOptions
						WHERE CustomProperty_ID = @propertyId;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@propertyId", SqlDbType.Int).Value = propertyId;

			var res = new List<CustomPropertyOption>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new CustomPropertyOption
					{
						CustomPropertyOptionId = (int)r["CustomPropertyOption_ID"],
						CustomPropertyId = (int)r["CustomProperty_ID"],
						Content = (string)r["Content"],
						IsEnabled = (bool)r["IsEnabled"],
						Sequence = (int)r["Sequence"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM CustomPropertyOptions WHERE CustomPropertyOption_ID = @ID;";

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

			_logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}