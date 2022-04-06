using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IGroupsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Group entry);

		Task<ServiceResponse<List<Group>>> GetAllAsync();

		Task<ServiceResponse<List<Group>>> GetUserGroupsAsync();
		
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
