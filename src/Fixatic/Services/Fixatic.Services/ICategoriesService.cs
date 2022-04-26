using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - ProjectCategory service
	/// </summary>
	public interface ICategoriesService
	{
		/// <summary>
		/// Creates the or update ProjectCategory.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(ProjectCategory entry);

		/// <summary>
		/// Gets all ProjectCategory.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<ProjectCategory>>> GetAllAsync();

		/// <summary>
		/// Deletes the ProjectCategory.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
