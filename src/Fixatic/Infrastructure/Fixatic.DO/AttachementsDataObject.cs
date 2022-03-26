using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class AttachementsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public AttachementsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Attachement attachement)
		{
			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = attachement.AttachementId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Attachements (content, name, size, type, uploaded, alternativetext, ticket_id, user_id, comment_id)
				VALUES (@content, @name, @size, @type, @uploaded, @alternativetext, @ticket_id, @user_id, @comment_id);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Attachements SET content = @content, name = @name, size = @size, type = @type, uploaded = @uploaded, alternativetext = @alternativetext, 
				ticket_id = @ticket_id, user_id = @user_id, comment_id = @comment_id WHERE Attachement_ID = @ID;";
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

			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Attachement_ID, Content, Name, Size, Type, Uploaded, AlternativeText, Ticket_ID, User_ID, Comment_ID FROM Attachements;";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					// TODO(Tom): přidat Attachement objekt
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Attachements WHERE Attachement_ID = @ID;";

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

			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}