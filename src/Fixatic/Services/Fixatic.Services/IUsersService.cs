using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - User service
	/// </summary>
	public interface IUsersService
	{
		/// <summary>
		/// Creates the or update User.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(User entry);

		/// <summary>
		/// Updates the sans password.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> UpdateSansPasswordAsync(User entry);

		/// <summary>
		/// Gets all Users.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<User>>> GetAllAsync();

		/// <summary>
		/// Gets the possible ticket assignees.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<BasicUserInfo>>> GetPossibleTicketAssigneesAsync(int ticketId);

		/// <summary>
		/// Gets User the by identifier.
		/// </summary>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<User>> GetByIdAsync(int userId);

		/// <summary>
		/// Deletes the User.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
