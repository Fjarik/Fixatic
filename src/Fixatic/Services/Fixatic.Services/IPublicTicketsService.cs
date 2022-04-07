using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IPublicTicketsService
	{
		Task<ServiceResponse<List<Ticket>?>> GetPublicAsync();

	}
}
