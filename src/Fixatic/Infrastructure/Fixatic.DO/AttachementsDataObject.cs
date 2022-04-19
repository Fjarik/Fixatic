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
				VALUES (@content, @name, @size, @type, SYSDATETIME(), @alternativetext, @ticket_id, @user_id, @comment_id);

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

			cmd.Parameters.Add("@content", SqlDbType.VarBinary).Value = attachement.Content;
			cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = attachement.Name;
			cmd.Parameters.Add("@size", SqlDbType.Int).Value = attachement.Size;
			cmd.Parameters.Add("@type", SqlDbType.VarChar).Value = attachement.Type;
			cmd.Parameters.Add("@uploaded", SqlDbType.DateTime2).Value = attachement.Uploaded;
			cmd.Parameters.Add("@alternativetext", SqlDbType.NVarChar).Value = attachement.AlternativeText ?? (object)DBNull.Value;
			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = attachement.TicketId ?? (object)DBNull.Value;
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = attachement.UserId;
			cmd.Parameters.Add("@comment_id", SqlDbType.Int).Value = attachement.CommentId ?? (object)DBNull.Value;

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

		public async Task<List<Attachement>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Attachement_ID, Content, Name, Size, Type, Uploaded, AlternativeText, Ticket_ID, User_ID, Comment_ID FROM Attachements;";

			var cmd = new SqlCommand(sql);

			var res = new List<Attachement>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Attachement
					{
						AttachementId = (int)r["Attachement_ID"],
						UserId = (int)r["User_ID"],
						TicketId = (int)r["Ticket_ID"],
						CommentId = (int)r["Comment_ID"],
						Content = (byte[])r["Content"],
						Name = (string)r["Name"],
						Size = (int)r["Size"],
						Type = (string)r["Type"],
						Uploaded = (DateTime)r["Uploaded"],
						AlternativeText = (string)r["AlternativeText"],
					});
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

		public async Task<List<Attachement>> GetByTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(GetByTicketAsync)}...");

			var sql = @"
				SELECT Attachement_ID, Content, Name, Size, Type, Uploaded, AlternativeText, Ticket_ID, User_ID, Comment_ID 
				FROM Attachements 
				WHERE Ticket_ID = @ticketId;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ticketId", SqlDbType.Int).Value = ticketId;

			var res = new List<Attachement>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Attachement
					{
						AttachementId = (int)r["Attachement_ID"],
						UserId = (int)r["User_ID"],
						TicketId = r["Ticket_ID"] as int?,
						CommentId = r["Comment_ID"] as int?,
						Content = (byte[])r["Content"],
						Name = (string)r["Name"],
						Size = (int)r["Size"],
						Type = (string)r["Type"],
						Uploaded = (DateTime)r["Uploaded"],
						AlternativeText = r["AlternativeText"] as string,
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(AttachementsDataObject)}.{nameof(GetByTicketAsync)}... Done");
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