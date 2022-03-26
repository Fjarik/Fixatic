using System.Data;
using System.Data.SqlClient;
using Fixatic.DO.Types;
using Fixatic.Types;
using Microsoft.Extensions.Logging;

namespace Fixatic.DO
{
    public class CustomPropertyValuesDataObject
    {
        public readonly DB _db;
        public readonly ILogger _logger;

        public CustomPropertyValuesDataObject(ILogger logger, IDBConnector dbConnector)
        {
            _db = new DB(dbConnector.GetCns());
            _logger = logger;
        }

        public async Task<int> CreateOrUpdateAsync(CustomPropertyValue customPropertyValue)
        {
            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(CreateOrUpdateAsync)}...");

            var id = customPropertyValue.CustomPropertyValueId;

            string sql;
            if (id == DB.IgnoredID)
            {
                sql = @"INSERT INTO CustomPropertyValues (created, custompropertyoption_id, ticket_id)
                VALUES (@created, @custompropertyoption_id, @ticket_id);";
            }
            else
            {
                //TODO: Vyřešit ID
                sql = @"UPDATE CustomPropertyValues SET created = @created, custompropertyoption_id = @custompropertyoption_id, ticket_id = @ticket_id;";
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

            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
            return id;
        }

        public async Task<List<User>> GetAllAsync()
        {
            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(GetAllAsync)}...");

            var sql = @"SELECT CustomPropertyOption_ID, Ticket_ID, Created FROM CustomPropertyValues;";

            var cmd = new SqlCommand(sql);

            var res = new List<User>();

            try
            {
                var r = await _db.ExecuteReaderAsync(cmd);

                while (await r.ReadAsync())
                {
                    // TODO(Tom): přidat CustomPropertyValue objekt
                }

                await r.CloseAsync();
            }
            finally
            {
                await cmd.Connection.CloseAsync();
            }

            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(GetAllAsync)}... Done");
            return res;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(DeleteAsync)}...");

            var sql = @"DELETE FROM CustomPropertyValues;";

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

            _logger.LogInformation($"{nameof(CustomPropertyValuesDataObject)}.{nameof(DeleteAsync)}... Done");
            return res != 0;
        }
    }
}