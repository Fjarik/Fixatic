using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Services.Types
{
	public sealed class ServiceResponse<T> : ServiceResponseBase
	{
		public T? Item { get; set; } = default;
	}

	public sealed class ServiceResponse : ServiceResponseBase
	{
	}

	public abstract class ServiceResponseBase
	{
		public bool IsSuccess { get; set; }
		public Exception? Exception { get; set; }

		public ServiceResponseBase()
		{
			IsSuccess = true;
			Exception = null;
		}

		public ServiceResponseBase(Exception exception)
		{
			IsSuccess = false;
			Exception = exception;
		}

		public void Fail(Exception exception)
		{
			IsSuccess = false;
			Exception = exception;
		}
	}
}
