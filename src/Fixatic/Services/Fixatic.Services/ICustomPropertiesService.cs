using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - CustomProperty service
	/// </summary>
	public interface ICustomPropertiesService
	{
		/// <summary>
		/// Creates the or update CustomProperty.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomProperty entry);

		/// <summary>
		/// Gets all CustomProperties.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<CustomProperty>>> GetAllAsync();

		/// <summary>
		/// Gets CustomProperty the by ticket.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<FullProperty>>> GetByTicketAsync(int ticketId);

		/// <summary>
		/// Deletes the CustomProperty.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
