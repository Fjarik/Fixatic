using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - Tickets service
	/// </summary>
	public interface ITicketsService
	{
		/// <summary>
		/// Creates the or update Ticket.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Ticket entry);

		/// <summary>
		/// Gets all Tickets.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<FullTicket>>> GetAllAsync();

		/// <summary>
		/// Gets Tickets the by project.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<FullTicket>>> GetByProjectAsync(int projectId);

		/// <summary>
		/// Gets the followed tickets.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<FullTicket>>> GetFollowedTicketsAsync();

		/// <summary>
		/// Gets the assigned tickets.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<FullTicket>>> GetAssignedTicketsAsync();

		/// <summary>
		/// Gets Ticket the by identifier.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<FullTicket?>> GetByIdAsync(int id);

		/// <summary>
		/// Deletes the Ticket.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);

		/// <summary>
		/// Sets the follow ticket.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="shouldFollow">if set to <c>true</c> [should follow].</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> SetFollowTicketAsync(int ticketId, bool shouldFollow);

		/// <summary>
		/// Determines whether Ticket is followed
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> IsFollowedAsync(int ticketId);

		/// <summary>
		/// Sets the assignee.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="userId">The user identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> SetAssigneeAsync(int ticketId, int userId);

		/// <summary>
		/// Gets the custom properties.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<FullTicketProperty>>> GetCustomPropertiesAsync(int ticketId);

		/// <summary>
		/// Adds the property option.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="propertyOptionId">The property option identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> AddPropertyOptionAsync(int ticketId, int propertyOptionId);

		/// <summary>
		/// Removes the property option.
		/// </summary>
		/// <param name="ticketId">The ticket identifier.</param>
		/// <param name="propertyOptionId">The property option identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> RemovePropertyOptionAsync(int ticketId, int propertyOptionId);

	}
}
