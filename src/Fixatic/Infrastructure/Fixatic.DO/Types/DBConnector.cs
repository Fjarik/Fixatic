using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.Types;

namespace Fixatic.DO.Types
{
	public class DBConnector : IDBConnector
	{
		private readonly string _connectionString;

		public DBConnector(ApplicationSettings settings) : this(settings.ConnectionString) { }

		public DBConnector(string connectionString)
		{
			_connectionString = connectionString;
		}

		public string GetCns()
		{
			return _connectionString;
		}
	}
}
