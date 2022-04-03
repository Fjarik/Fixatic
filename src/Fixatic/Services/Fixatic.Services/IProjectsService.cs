using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface IProjectsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Project entry);

		Task<ServiceResponse<List<Project>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
