using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - Public ticket service
	/// </summary>
	public interface IPublicTicketsService
	{
		/// <summary>
		/// Gets the public tickets.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Ticket>>> GetPublicAsync();

	}
}
