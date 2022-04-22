using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ITicketsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Ticket entry);

		Task<ServiceResponse<List<FullTicket>>> GetAllAsync();

		Task<ServiceResponse<List<FullTicket>>> GetByProjectAsync(int projectId);

		Task<ServiceResponse<List<FullTicket>>> GetFollowedTicketsAsync();
		
		Task<ServiceResponse<List<FullTicket>>> GetAssignedTicketsAsync();

		Task<ServiceResponse<FullTicket?>> GetByIdAsync(int id);

		Task<ServiceResponse<bool>> DeleteAsync(int id);

		Task<ServiceResponse<bool>> SetFollowTicketAsync(int ticketId, bool shouldFollow);

		Task<ServiceResponse<bool>> IsFollowedAsync(int ticketId);
		
		Task<ServiceResponse<bool>> SetAssigneeAsync(int ticketId, int userId);

		Task<ServiceResponse<List<FullTicketProperty>>> GetCustomPropertiesAsync(int ticketId);

		Task<ServiceResponse<bool>> AddPropertyOptionAsync(int ticketId, int propertyOptionId);

		Task<ServiceResponse<bool>> RemovePropertyOptionAsync(int ticketId, int propertyOptionId);

	}
}
