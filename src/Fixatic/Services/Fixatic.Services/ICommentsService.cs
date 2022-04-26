using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - Comment service
	/// </summary>
	public interface ICommentsService
	{
		/// <summary>
		/// Creates the or update Comment.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Comment entry);

		/// <summary>
		/// Gets all Comment.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Comment>>> GetAllAsync();

		/// <summary>
		/// Gets the by ticket Comment.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<Comment>>> GetByTicketAsync(int ticketId);

		/// <summary>
		/// Gets Comment by ticket user visibility.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<Comment>>> GetByTicketUserVisibleAsync(int ticketId);

		/// <summary>
		/// Deletes the Comment.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
