using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IProjectsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Project entry);

		Task<ServiceResponse<Project>> GetByIdAsync(int projectId);

		Task<ServiceResponse<List<Project>>> GetAllAsync();

		Task<ServiceResponse<List<Project>>> GetGroupProjectsAsync(int groupGroupId);

		Task<ServiceResponse<List<int>>> GetCategoryIdsAsync(int projectId);

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
