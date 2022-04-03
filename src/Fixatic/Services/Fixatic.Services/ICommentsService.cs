using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICommentsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Comment entry);

		Task<ServiceResponse<List<Comment>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
