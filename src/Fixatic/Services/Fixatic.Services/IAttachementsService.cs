using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - attachements service
	/// </summary>
	public interface IAttachementsService
	{
		/// <summary>
		/// Creates the or update attachement.
		/// </summary>
		/// <param name="entry">The Attachement.</param>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Attachement entry);

		/// <summary>
		/// Gets all Attachements.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Attachement>>> GetAllAsync();

		/// <summary>
		/// Gets Attachement the by ticket.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<Attachement>>> GetByTicketAsync(int ticketId);

		/// <summary>
		/// Deletes the Attachement.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
