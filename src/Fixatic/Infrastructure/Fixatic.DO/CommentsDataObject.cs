using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class CommentsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public CommentsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Comment comment)
		{
			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = comment.CommentId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Comments (content, created, isinternal, ticket_id, user_id)
				VALUES (@content, @created, @isinternal, @ticket_id, @user_id);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Comments SET content = @content, created = @created, isinternal = @isinternal, ticket_id = @ticket_id, user_id = @user_id
				WHERE comment_id = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@content", SqlDbType.NText).Value = comment.Content;
			cmd.Parameters.Add("@created", SqlDbType.DateTime2).Value = comment.Created;
			cmd.Parameters.Add("@isinternal", SqlDbType.Bit).Value = comment.IsInternal;
			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = comment.TicketId;
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = comment.UserId;
			
			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null) id = (int)objId;
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<Comment>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Comment_ID, Content, Created, IsInternal, Ticket_ID, User_ID FROM Comments;";

			var cmd = new SqlCommand(sql);

			var res = new List<Comment>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Comment
					{
						CommentId = (int)r["Comment_ID"],
						TicketId = (int)r["Ticket_ID"],
						UserId = (int)r["User_ID"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						IsInternal = (bool)r["IsInternal"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Comments WHERE comment_id = @ID;";

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

			_logger.LogInformation($"{nameof(CommentsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}