using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
	public class GroupsDataObject
	{
		public readonly DB _db;
		public readonly ILogger _logger;

		public GroupsDataObject(ILogger logger, IDBConnector dbConnector)
		{
			_db = new DB(dbConnector.GetCns());
			_logger = logger;
		}

		public async Task<int> CreateOrUpdateAsync(Group group)
		{
			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

			var id = group.GroupId;

			string sql;
			if (id == DB.IgnoredID)
			{
				sql = @"INSERT INTO Groups (description, name, type)
				VALUES (@description, @name, @type);
				
				SET @ID = SCOPE_IDENTITY();

                SELECT @ID;";
			}
			else
			{
				sql = @"UPDATE Groups SET description = @description, name = @name, type = @type WHERE Group_ID = @ID;";
			}

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;

			cmd.Parameters.Add("@description", SqlDbType.NVarChar).Value = group.Description;
			cmd.Parameters.Add("@name", SqlDbType.NVarChar).Value = group.Name;
			cmd.Parameters.Add("@type", SqlDbType.Int).Value = (int)group.Type;

			try
			{
				var objId = await _db.ExecuteScalarAsync(cmd);
				if (objId != null) id = (int)objId;
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
			return id;
		}

		public async Task<List<Group>> GetAllAsync()
		{
			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(GetAllAsync)}...");

			var sql = @"SELECT Group_ID, Name, Type, Description FROM Groups;";

			var cmd = new SqlCommand(sql);

			var res = new List<Group>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Group
					{
						GroupId = (int)r["Group_ID"],
						Description = (string)r["Description"],
						Name = (string)r["Name"],
						Type = (UserGroupType)(int)r["Type"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(GetAllAsync)}... Done");
			return res;
		}

		public async Task<List<Group>> GetUserGroups(int userId)
		{
			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(GetUserGroups)}...");

			// TODO: tady lze použít pohled
			var sql = @"
				SELECT g.Group_ID, Name, Type, Description
				FROM Groups g
				INNER JOIN UsersGroups UG on g.Group_ID = UG.Group_ID
				WHERE UG.User_ID = @userid
			;";

			var cmd = new SqlCommand(sql);

			cmd.Parameters.Add("@userid", SqlDbType.Int).Value = userId;

			var res = new List<Group>();

			try
			{
				var r = await _db.ExecuteReaderAsync(cmd);

				while (await r.ReadAsync())
				{
					res.Add(new Group
					{
						GroupId = (int)r["Group_ID"],
						Description = (string)r["Description"],
						Name = (string)r["Name"],
						Type = (UserGroupType)(int)r["Type"]
					});
				}

				await r.CloseAsync();
			}
			finally
			{
				await cmd.Connection.CloseAsync();
			}

			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(GetUserGroups)}... Done");
			return res;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(DeleteAsync)}...");

			var sql = @"DELETE FROM Groups WHERE Group_ID = @ID;";

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

			_logger.LogInformation($"{nameof(GroupsDataObject)}.{nameof(DeleteAsync)}... Done");
			return res != 0;
		}
	}
}