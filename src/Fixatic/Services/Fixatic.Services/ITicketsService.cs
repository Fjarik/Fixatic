using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ITicketsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Ticket entry);

		Task<ServiceResponse<List<Ticket>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
