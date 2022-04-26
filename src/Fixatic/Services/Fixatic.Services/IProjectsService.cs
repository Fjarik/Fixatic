using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - Project service
	/// </summary>
	public interface IProjectsService
	{
		/// <summary>
		/// Creates the or update Project.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Project entry);

		/// <summary>
		/// Gets Project the by identifier.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<Project?>> GetByIdAsync(int projectId);

		/// <summary>
		/// Gets all ProjectsProject.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Project>>> GetAllAsync();

		/// <summary>
		/// Gets the group projects.
		/// </summary>
		/// <param name="groupGroupId">The group group identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<Project>>> GetGroupProjectsAsync(int groupGroupId);

		/// <summary>
		/// Gets the category ids.
		/// </summary>
		/// <param name="projectId">The project identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<int>>> GetCategoryIdsAsync(int projectId);

		/// <summary>
		/// Deletes the Project.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
