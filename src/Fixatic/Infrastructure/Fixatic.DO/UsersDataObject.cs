using Fixatic.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<int> CreateOrUpdateAsync()
        {
            _logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(CreateOrUpdateAsync)}...");

            var id = user.Id;

            string sql;


            var cmd = new SqlCommand();

            var objId = await _db.ExecuteScalarAsync(cmd);
            if (objId != null)
            {
                id = (int)objId;
            }
            await cmd.Connection.CloseAsync();

            _logger.LogInformation($"{nameof(UsersDataObject)}.{nameof(CreateOrUpdateAsync)}... Done");
            return id;
        }
    }
}
