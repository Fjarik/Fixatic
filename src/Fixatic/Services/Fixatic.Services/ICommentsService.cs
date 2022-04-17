using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICommentsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Comment entry);

		Task<ServiceResponse<List<Comment>>> GetAllAsync();

		Task<ServiceResponse<List<Comment>>> GetByTicketAsync(int ticketId);
		
		Task<ServiceResponse<List<Comment>>> GetByTicketUserVisibleAsync(int ticketId);

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
