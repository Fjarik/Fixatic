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
                    INSERT INTO Users (firstname, lastname, email, password, phone, created, isenabled)
                    VALUES (@firstname, @lastname, @email, @password, @phone, @created, @isenabled);

                    SET @ID = SCOPE_IDENTITY();

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
                    IsEnabled
                FROM Users
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
