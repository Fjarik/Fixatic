using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICategoriesService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(ProjectCategory entry);

		Task<ServiceResponse<List<ProjectCategory>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
