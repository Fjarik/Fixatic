using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class FollowersDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public FollowersDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Follower follower)
		{
			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = 69;
			var userId = follower.UserId;
			var ticketId = follower.TicketId;

			if (userId == DB.IgnoredID || ticketId == DB.IgnoredID)
			{
				throw new ArgumentException("Follower object must have a valid UserID and TicketID");
			}

			// TODO: tady asi bude jiná logika na zkontrolování, jestli se má záznam vytvořit nebo updatovat

			string sql;
			if (userId == DB.IgnoredID)
			{
				sql = @"INSERT INTO Followers (since, type, ticket_id, user_id)
				VALUES (@since, @type, @ticket_id, @user_id);";
			}
			else
			{
				//TODO: Vyřešit ID
				sql = @"UPDATE Followers SET since = @since, type = @type, ticket_id = @ticket_id, user_id = @user_id;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = id;
			cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = id;
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

			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Ticket_ID, User_ID, Type, Since FROM Followers;";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					// TODO(Tom): přidat Follower objekt
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int userId, int ticketId)
		{
			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Followers;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userId;
			cmd.Parameters.Add("@TicketID", SqlDbType.Int).Value = ticketId;

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}