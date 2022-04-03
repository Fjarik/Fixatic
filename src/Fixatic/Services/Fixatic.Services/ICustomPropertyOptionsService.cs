using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICustomPropertyOptionsService
	{
		Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomPropertyOption entry);

		Task<ServiceResponse<List<CustomPropertyOption>>> GetAllAsync();

		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
