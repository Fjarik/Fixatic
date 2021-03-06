using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Data.SqlClient;

namespace Fixatic.DO
{
	public class UsersDataObject
	{
		public readonly ILogger _logger;
		public readonly DB _db;

		public UsersDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = user.UserId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"
					EXEC dbo.proc_create_user @Email = @email, @fname= @firstname , @lname = @lastname, @pwdHash = @password, @phone = @phone, @NewID = @ID OUTPUT;

                    SELECT @ID;
                ";
			}
			else
			{
				sql = @"
                    UPDATE Users
                    SET
                        firstname = @firstname,
                        lastname = @lastname,
                        email = @email,
                        password = @password,
                        phone = @phone,
                        created = @created,
                        isenabled = @isenabled
                    WHERE User_ID = @ID;
                ";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = user.Firstname;
			cmd.Parameters.Add("@lastname", SqlDbType.NVarChar).Value = user.Lastname;
			cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = user.Email;
			cmd.Parameters.Add("@password", SqlDbType.NVarChar).Value = user.Password;
			cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = user.Phone;
			cmd.Parameters.Add("@created", SqlDbType.DateTime2).Value = user.Created;
			cmd.Parameters.Add("@isenabled", SqlDbType.Bit).Value = user.IsEnabled;

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null)
				{
					id = (int)objId;
				}
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<int> UpdateSansPasswordAsync(User user)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(UpdateSansPasswordAsync)}...");

			var id = user.UserId;

			var sql = @"
                    UPDATE Users
                    SET
                        firstname = @firstname,
                        lastname = @lastname,
                        email = @email,
                        phone = @phone,
                        created = @created,
                        isenabled = @isenabled
                    WHERE User_ID = @ID;
                ";

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@firstname", SqlDbType.NVarChar).Value = user.Firstname;
			cmd.Parameters.Add("@lastname", SqlDbType.NVarChar).Value = user.Lastname;
			cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = user.Email;
			cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = user.Phone;
			cmd.Parameters.Add("@created", SqlDbType.DateTime2).Value = user.Created;
			cmd.Parameters.Add("@isenabled", SqlDbType.Bit).Value = user.IsEnabled;

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null)
				{
					id = (int)objId;
				}
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(UpdateSansPasswordAsync)}... Done");
			return id;
		}

		public async Task<List<User>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"
                SELECT
                    User_ID,
                    Firstname,
                    Lastname,
                    Email,
                    Phone,
                    Created,
                    IsEnabled
                FROM Users;
            ";

			var cmd = new SqlCommand(sql);

			var res = new List<User>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new User
					{
						UserId = (int)r["User_ID"],
						Firstname = (string)r["Firstname"],
						Lastname = (string)r["Lastname"],
						Email = (string)r["Email"],
						Phone = (string)r["Phone"],
						Password = null,
						Created = (DateTime)r["Created"],
						IsEnabled = (bool)r["IsEnabled"],
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<List<BasicUserInfo>> GetPossibleTicketAssigneesAsync(int ticketId)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetPossibleTicketAssigneesAsync)}...");

			var sql = @"
                SELECT DISTINCT U.User_ID, U.Firstname, U.Lastname FROM Tickets T
					INNER JOIN ProjectsAccess Pa ON T.Project_ID = Pa.Project_ID
					INNER JOIN UsersGroups Ug ON Pa.Group_ID = Ug.Group_ID
					INNER JOIN Users U ON Ug.User_ID = U.User_ID
					WHERE T.Ticket_ID = @ID AND U.IsEnabled = 1 AND dbo.fn_user_type(u.User_ID) = 2;
            ";

			var cmd = new SqlCommand(sql);
			
			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = ticketId;

			var res = new List<BasicUserInfo>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new BasicUserInfo
					{
						UserId = (int)r["User_ID"],
						Firstname = (string)r["Firstname"],
						Lastname = (string)r["Lastname"],
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetPossibleTicketAssigneesAsync)}... Done");
			return res;
		}

		public async Task<User?> GetByIdAsync(int userId)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetByIdAsync)}...");

			var sql = @"
                SELECT
                    User_ID,
                    Firstname,
                    Lastname,
                    Email,
                    Phone,
                    Created,
                    IsEnabled
                FROM Users
				WHERE User_ID = @ID;
            ";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = userId;

			User? res = null;

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				if (await r.ReadAsync())
				{
					res = new User
					{
						UserId = (int)r["User_ID"],
						Firstname = (string)r["Firstname"],
						Lastname = (string)r["Lastname"],
						Email = (string)r["Email"],
						Phone = (string)r["Phone"],
						Password = null,
						Created = (DateTime)r["Created"],
						IsEnabled = (bool)r["IsEnabled"],
					};
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetByIdAsync)}... Done");
			return res;
		}

		public async Task<User?> GetUserWithPasswordAsync(string email)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetUserWithPasswordAsync)}...");

			var sql = @"
                SELECT
                    User_ID,
                    Firstname,
                    Lastname,
                    Email,
					Password,
                    Phone,
                    Created,
                    IsEnabled
                FROM Users
				WHERE Email = @email;
            ";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

			User? res = null;

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				if (await r.ReadAsync())
				{
					res = new User
					{
						UserId = (int)r["User_ID"],
						Firstname = (string)r["Firstname"],
						Lastname = (string)r["Lastname"],
						Email = (string)r["Email"],
						Password = (string)r["Password"],
						Phone = (string)r["Phone"],
						Created = (DateTime)r["Created"],
						IsEnabled = (bool)r["IsEnabled"],
					};
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetUserWithPasswordAsync)}... Done");
			return res;
		}

		public async Task<CurrentUser?> GetCurrentUserAsync(string email)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetCurrentUserAsync)}...");

			var sql = @"
                SELECT
                    User_ID,
                    Firstname,
                    Lastname,
                    Email,
                    Phone,
                    Created,
                    IsEnabled,
					Type
                FROM view_full_users
				WHERE Email = @email;
            ";

			var cmd = new SqlCommand(sql);
			cmd.Parameters.Add("@email", SqlDbType.NVarChar).Value = email;

			CurrentUser? res = null;

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				if (await r.ReadAsync())
				{
					res = new CurrentUser
					{
						UserId = (int)r["User_ID"],
						Firstname = (string)r["Firstname"],
						Lastname = (string)r["Lastname"],
						Email = (string)r["Email"],
						Password = null,
						Phone = (string)r["Phone"],
						Created = (DateTime)r["Created"],
						IsEnabled = (bool)r["IsEnabled"],
						GroupType = (UserGroupType)(int)r["Type"],
					};
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(GetCurrentUserAsync)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"
                DELETE FROM Users
                WHERE User_ID = @ID;
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

			_logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}