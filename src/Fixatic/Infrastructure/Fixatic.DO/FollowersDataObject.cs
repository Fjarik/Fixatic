using System.Data;
using System.Data.SqlClient;
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

        async public Task<int> CreateOrUpdateAsync(Follower follower)
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
            
            string sql = "";
            if (userId == DB.IgnoredID)
            {
                // TODO(Dan): SQL INSERT
                sql = @"";
            }
            else
            {
                // TODO(Dan): SQL UPDATE
                sql = @"";
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

        async public Task<List<User>> GetAllAsync()
        {
            _logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(GetAllAsync)}...");

            // TODO(Dan): SQL SELECT
            var sql = @"";

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

        async public Task<bool> DeleteAsync(int userId, int ticketId)
        {
            _logger.LogInformation($"{nameof(FollowersDataObject)}.{nameof(DeleteAsync)}...");

            // TODO(Dan): SQL DELETE
            var sql = @"";

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