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

			var isClosed = ticket.Status == TicketStatus.Closed || ticket.Status == TicketStatus.Done;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql =
					@"INSERT INTO Tickets (content, created, datesolved, modified, priority, status, title, type, visibility, project_id, assigneduser_id, creator_id)
									VALUES (@content, @created, @datesolved, @modified, @priority, @status, @title, @type, @visibility, @project_id, @assigneduser_id, @creator_id);

				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Tickets
						SET
							content = @content,
							datesolved = @datesolved,
							modified = @modified,
							priority = @priority,
							status = @status,
							title = @title,
							type = @type,
							visibility = @visibility,
							project_id = @project_id,
							assigneduser_id = @assigneduser_id
						WHERE ticket_id = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@content", SqlDbType.NText).Value = ticket.Content;
			cmd.Parameters.Add("@created", SqlDbType.DateTime2).Value = ticket.Created;
			cmd.Parameters.Add("@datesolved", SqlDbType.DateTime2).Value = (isClosed && ticket.DateSolved == null)
				? DateTime.Now
				: (ticket.DateSolved ?? (object)DBNull.Value);
			cmd.Parameters.Add("@modified", SqlDbType.DateTime2).Value = DateTime.Now;
			cmd.Parameters.Add("@priority", SqlDbType.Int).Value = (int)ticket.Priority;
			cmd.Parameters.Add("@status", SqlDbType.Int).Value = (int)ticket.Status;
			cmd.Parameters.Add("@title", SqlDbType.NVarChar).Value = ticket.Title;
			cmd.Parameters.Add("@type", SqlDbType.Int).Value = (int)ticket.Type;
			cmd.Parameters.Add("@visibility", SqlDbType.Int).Value = (int)ticket.Visibility;
			cmd.Parameters.Add("@project_id", SqlDbType.Int).Value = ticket.ProjectId;
			cmd.Parameters.Add("@assigneduser_id", SqlDbType.Int).Value = ticket.AssignedUserId ?? (object)DBNull.Value;
			cmd.Parameters.Add("@creator_id", SqlDbType.Int).Value = ticket.CreatorId;

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

		public async Task<List<FullTicket>> GetAllAsync(int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"
				SELECT
					Ticket_ID, 
					Project_ID, 
					AssignedUser_ID, 
					Creator_ID,
					Title, 
					Content, 
					Created, 
					Modified, 
					DateSolved, 
					Priority, 
					Status, 
					Type, 
					Visibility, 
					Followers,
					AssigneeName
				FROM view_tickets
				WHERE Project_ID IN (
					SELECT DISTINCT
						pa.Project_ID
					FROM ProjectsAccess pa
					INNER JOIN UsersGroups ug ON ug.Group_ID = pa.Group_ID
					WHERE ug.User_ID = @userId
				);
			";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;


			var res = new List<FullTicket>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new FullTicket
					{
						TicketId = (int)r["Ticket_ID"],
						ProjectId = (int)r["Project_ID"],
						AssignedUserId = r["AssignedUser_ID"] as int?,
						CreatorId = (int)r["Creator_ID"],
						Title = (string)r["Title"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						Modified = r["Modified"] as DateTime?,
						DateSolved = r["DateSolved"] as DateTime?,
						Priority = (TicketPriority)(int)r["Priority"],
						Status = (TicketStatus)(int)r["Status"],
						Type = (TicketType)(int)r["Type"],
						Visibility = (TicketVisibility)(int)r["Visibility"],
						Followers = (int)r["Followers"],
						AssigneeName = (r["AssigneeName"] as string) ?? string.Empty
					});
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

		public async Task<List<FullTicket>> GetFollowedTicketsAsync(int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowedTicketsAsync)}...");

			var sql = @"
				SELECT
					t.Ticket_ID, 
					t.Project_ID, 
					t.AssignedUser_ID, 
					t.Creator_ID,
					t.Title, 
					t.Content, 
					t.Created, 
					t.Modified, 
					t.DateSolved, 
					t.Priority, 
					t.Status, 
					t.Type, 
					t.Visibility, 
					t.Followers,
					t.AssigneeName
				FROM view_tickets t
				INNER JOIN Followers f ON f.Ticket_ID = t.Ticket_ID
				WHERE f.User_ID = @userId
			";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@userId", SqlDbType.Int).Value = userId;

			var res = new List<FullTicket>();
			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new FullTicket
					{
						TicketId = (int)r["Ticket_ID"],
						ProjectId = (int)r["Project_ID"],
						AssignedUserId = r["AssignedUser_ID"] as int?,
						CreatorId = (int)r["Creator_ID"],
						Title = (string)r["Title"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						Modified = r["Modified"] as DateTime?,
						DateSolved = r["DateSolved"] as DateTime?,
						Priority = (TicketPriority)(int)r["Priority"],
						Status = (TicketStatus)(int)r["Status"],
						Type = (TicketType)(int)r["Type"],
						Visibility = (TicketVisibility)(int)r["Visibility"],
						Followers = (int)r["Followers"],
						AssigneeName = (r["AssigneeName"] as string) ?? string.Empty
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowedTicketsAsync)}... Done");
			return res;
		}

		public async Task<FullTicket?> GetByIdAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByIdAsync)}...");

			var sql = @"
				SELECT
					Ticket_ID, 
					Project_ID, 
					AssignedUser_ID, 
					Creator_ID,
					Title, 
					Content, 
					Created, 
					Modified, 
					DateSolved, 
					Priority, 
					Status, 
					Type, 
					Visibility, 
					Followers,
					AssigneeName
				FROM view_tickets
				WHERE Ticket_ID = @ID;
			";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			FullTicket? res = null;

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				if (await r.ReadAsync())
				{
					res = new FullTicket
					{
						TicketId = (int)r["Ticket_ID"],
						ProjectId = (int)r["Project_ID"],
						AssignedUserId = r["AssignedUser_ID"] as int?,
						CreatorId = (int)r["Creator_ID"],
						Title = (string)r["Title"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						Modified = r["Modified"] as DateTime?,
						DateSolved = r["DateSolved"] as DateTime?,
						Priority = (TicketPriority)(int)r["Priority"],
						Status = (TicketStatus)(int)r["Status"],
						Type = (TicketType)(int)r["Type"],
						Visibility = (TicketVisibility)(int)r["Visibility"],
						Followers = (int)r["Followers"],
						AssigneeName = (r["AssigneeName"] as string) ?? string.Empty
					};
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		public async Task<List<FullTicket>> GetByProjectAsync(int projectId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByProjectAsync)}...");

			var sql = @"
				SELECT
					Ticket_ID, 
					Project_ID, 
					AssignedUser_ID, 
					Creator_ID,
					Title, 
					Content, 
					Created, 
					Modified, 
					DateSolved, 
					Priority, 
					Status, 
					Type, 
					Visibility, 
					Followers,
					AssigneeName
				FROM view_tickets
				WHERE Project_ID = @projectId;
			";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@projectId", SqlDbType.Int).Value = projectId;

			var res = new List<FullTicket>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new FullTicket
					{
						TicketId = (int)r["Ticket_ID"],
						ProjectId = (int)r["Project_ID"],
						AssignedUserId = r["AssignedUser_ID"] as int?,
						CreatorId = (int)r["Creator_ID"],
						Title = (string)r["Title"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						Modified = r["Modified"] as DateTime?,
						DateSolved = r["DateSolved"] as DateTime?,
						Priority = (TicketPriority)(int)r["Priority"],
						Status = (TicketStatus)(int)r["Status"],
						Type = (TicketType)(int)r["Type"],
						Visibility = (TicketVisibility)(int)r["Visibility"],
						Followers = (int)r["Followers"],
						AssigneeName = (r["AssigneeName"] as string) ?? string.Empty
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByProjectAsync)}... Done");
			return res;
		}

		public async Task<List<Ticket>> GetByVisibilityAsync(TicketVisibility visibility)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByVisibilityAsync)}...");

			var sql = @"SELECT
							Ticket_ID, 
							Title, 
							Content, 
							Created, 
							Modified, 
							DateSolved, 
							Priority, 
							Status, 
							Type, 
							Visibility, 
							Project_ID, 
							AssignedUser_ID, 
							Creator_ID
						FROM Tickets
						WHERE Visibility = @visibility;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@visibility", SqlDbType.Int).Value = (int)visibility;

			var res = new List<Ticket>();
			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Ticket
					{
						TicketId = (int)r["Ticket_ID"],
						ProjectId = (int)r["Project_ID"],
						AssignedUserId = r["AssignedUser_ID"] as int?,
						CreatorId = (int)r["Creator_ID"],
						Content = (string)r["Content"],
						Created = (DateTime)r["Created"],
						DateSolved = r["DateSolved"] as DateTime?,
						Modified = r["Modified"] as DateTime?,
						Priority = (TicketPriority)(int)r["Priority"],
						Status = (TicketStatus)(int)r["Status"],
						Title = (string)r["Title"],
						Type = (TicketType)(int)r["Type"],
						Visibility = (TicketVisibility)(int)r["Visibility"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetByVisibilityAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"
				DELETE FROM Followers WHERE Ticket_ID = @ID;
				DELETE FROM Comments WHERE Ticket_ID = @ID;
				DELETE FROM CustomPropertyValues WHERE Ticket_ID = @ID;
				DELETE FROM Attachements WHERE Ticket_ID = @ID;
				DELETE FROM Tickets WHERE Ticket_ID = @ID;
			";

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

		public async Task<bool> FinishTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(FinishTicketAsync)}...");

			var sql = @"
				EXECUTE proc_finish_ticket @ticketId;
			";

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;
			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(FinishTicketAsync)}... Done");
			return res != 0;
		}

		public async Task<List<Follower>> GetFollowerByUserAsync(int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerByUserAsync)}...");

			var sql = @"SELECT Ticket_ID, User_ID, Type, Since FROM Followers WHERE User_ID = @user_id;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;

			var res = new List<Follower>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Follower
					{
						UserId = (int)r["User_ID"],
						TicketId = (int)r["Ticket_ID"],
						Since = (DateTime)r["Since"],
						Type = (int)r["Type"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerByUserAsync)}... Done");
			return res;
		}

		public async Task<List<Follower>> GetFollowerByTicketAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerByTicketAsync)}...");

			var sql = @"SELECT Ticket_ID, User_ID, Type, Since FROM Followers WHERE Ticket_ID = @ticket_id;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;

			var res = new List<Follower>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Follower
					{
						UserId = (int)r["User_ID"],
						TicketId = (int)r["Ticket_ID"],
						Since = (DateTime)r["Since"],
						Type = (int)r["Type"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerByTicketAsync)}... Done");
			return res;
		}

		public async Task<Follower?> GetFollowerAsync(int ticketId, int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerAsync)}...");

			var sql =
				@"SELECT Ticket_ID, User_ID, Type, Since FROM Followers WHERE Ticket_ID = @ticket_id AND User_ID = @user_id;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;

			Follower? res = null;

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				if (await r.ReadAsync())
				{
					res = new Follower
					{
						UserId = (int)r["User_ID"],
						TicketId = (int)r["Ticket_ID"],
						Since = (DateTime)r["Since"],
						Type = (int)r["Type"]
					};
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetFollowerAsync)}... Done");
			return res;
		}

		public async Task<bool> AddFollowerAsync(int tickedId, int userId, int type)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(AddFollowerAsync)}...");

			var sql = @"
				INSERT INTO Followers (since, type, ticket_id, user_id)
				VALUES (@since, @type, @ticket_id, @user_id);";


			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = tickedId;
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;
			cmd.Parameters.Add("@type", SqlDbType.Int).Value = type;
			cmd.Parameters.Add("@since", SqlDbType.DateTime2).Value = DateTime.UtcNow;

			try
			{
				await _db.ExecuteScalarAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(AddFollowerAsync)}... Done");
			return true;
		}

		public async Task<bool> RemoveFollowerAsync(int ticketId, int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(RemoveFollowerAsync)}...");

			var sql = @"
				DELETE FROM Followers WHERE Ticket_ID = @ticket_id AND User_ID = @user_id;";


			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;
			cmd.Parameters.Add("@user_id", SqlDbType.Int).Value = userId;

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(RemoveFollowerAsync)}... Done");
			return res != 0;
		}

		public async Task<bool> SetAssigneeAsync(int ticketId, int userId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(SetAssigneeAsync)}...");

			var sql = @"UPDATE Tickets
						SET
							assigneduser_id = @assigneduser_id
						WHERE ticket_id = @ID;";

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ticketId;
			cmd.Parameters.Add("@assigneduser_id", SqlDbType.Int).Value = userId;

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(SetAssigneeAsync)}... Done");
			return res != 0;
		}

		public async Task<List<int>> GetCustomPropertyOptionIdsAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetCustomPropertyOptionIdsAsync)}...");

			var sql = @"SELECT CustomPropertyOption_ID FROM CustomPropertyValues WHERE Ticket_ID = @ticket_id;";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;

			var res = new List<int>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add((int)r["CustomPropertyOption_ID"]);
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(GetCustomPropertyOptionIdsAsync)}... Done");
			return res;
		}

		public async Task<bool> AddPropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(AddPropertyOptionAsync)}...");

			var sql = @"
				INSERT INTO CustomPropertyValues (created, custompropertyoption_id, ticket_id)
				VALUES (@created, @custompropertyoption_id, @ticket_id);";


			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;
			cmd.Parameters.Add("@custompropertyoption_id", SqlDbType.Int).Value = propertyOptionId;
			cmd.Parameters.Add("@created", SqlDbType.DateTime2).Value = DateTime.UtcNow;

			try
			{
				await _db.ExecuteScalarAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(AddPropertyOptionAsync)}... Done");
			return true;
		}

		public async Task<bool> RemovePropertyOptionAsync(int ticketId, int propertyOptionId)
		{
			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(RemovePropertyOptionAsync)}...");

			var sql = @"
				DELETE FROM CustomPropertyValues WHERE Ticket_ID = @ticket_id AND CustomPropertyOption_ID = @custompropertyoption_id;
			";


			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ticket_id", SqlDbType.Int).Value = ticketId;
			cmd.Parameters.Add("@custompropertyoption_id", SqlDbType.Int).Value = propertyOptionId;

			int res;
			try
			{
				res = await _db.ExecuteNonQueryAsync(cmd);
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(TicketsDataObject)}.{nameof(RemovePropertyOptionAsync)}... Done");
			return res != 0;
		}
	}
}