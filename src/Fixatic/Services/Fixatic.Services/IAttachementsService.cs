using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IAttachementsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Attachement entry);

		Task<ServiceResponse<List<Attachement>>> GetAllAsync();
		
		Task<ServiceResponse<List<Attachement>>> GetByTicketAsync(int ticketId);

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
