using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Services.Types
{
	/// <summary>
	/// Service response
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <seealso cref="Fixatic.Services.Types.ServiceResponseBase" />
	public sealed class ServiceResponse<T> : ServiceResponseBase
	{
		public T? Item { get; set; } = default;
	}

	/// <summary>
	/// Service response
	/// </summary>
	/// <seealso cref="Fixatic.Services.Types.ServiceResponseBase" />
	public sealed class ServiceResponse : ServiceResponseBase
	{
	}

	/// <summary>
	/// Service response
	/// </summary>
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
