using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class TicketsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public TicketsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Ticket ticket)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = ticket.TicketId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Tickets (content, created, datesolved, modified, priority, status, title, type, visibility, project_id, assigneduser_id, creator_id)
				VALUES (@content, @created, @datesolved, @modified, @priority, @status, @title, @type, @visibility, @project_id, @assigneduser_id, @creator_id);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Tickets SET content = @content, created = @created, datesolved = @datesolved, modified = @modified, priority = @priority, status = @status,
				title = @title, type = @type, visibility = @visibility, project_id = @project_id, assigned_user = @assigned_user, creator_id = @creator_id WHERE ticket_id = @ID;";
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

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Ticket_ID, Title, Content, Created, Modified, DateSolved, Priority, Status, Type, Visibility, Project_ID, AssignedUser_ID, Creator_ID
			FROM Tickets;";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					// TODO(Tom): přidat Ticket objekt
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Tickets WHERE Ticket_ID = @ID;";

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

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}