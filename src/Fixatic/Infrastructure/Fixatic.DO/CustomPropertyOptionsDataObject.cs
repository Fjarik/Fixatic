using System.Data;
using System.Data.SqlClient;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
    public class CustomPropertyOptionsDataObject
    {
        public readonly DB _db;
        public readonly ILogger _logger;

        public CustomPropertyOptionsDataObject(ILogger logger, IDBConnector dbConnector)
        {
            _db = new DB(dbConnector.GetCns());
            _logger = logger;
        }

        async public Task<int> CreateOrUpdateAsync(CustomPropertyOption customPropertyOption)
        {
            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(CreateOrUpdateAsync)}...");

            var id = customPropertyOption.CustomPropertyOptionId;

            string sql;
            if (id == DB.IgnoredID)
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

            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
            return id;
        }

        async public Task<List<User>> GetAllAsync()
        {
            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(GetAllAsync)}...");

            // TODO(Dan): SQL SELECT
            var sql = @"";

            var cmd = new SqlCommand(sql);

            var res = new List<User>();

            try
            {
                var r = await _db.ExecuteReaderAsync(cmd);

                while (await r.ReadAsync())
                {
                    // TODO(Tom): přidat CustomPropertyOption objekt
                }

                await r.CloseAsync();
            }
            finally
            {
                await cmd.Connection.CloseAsync();
            }

            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(GetAllAsync)}... Done");
            return res;
        }

        async public Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(DeleteAsync)}...");

            // TODO(Dan): SQL DELETE
            var sql = @"";

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

            _logger.LogInformation($"{nameof(CustomPropertyOptionsDataObject)}.{nameof(DeleteAsync)}... Done");
            return res != 0;
        }
    }
}