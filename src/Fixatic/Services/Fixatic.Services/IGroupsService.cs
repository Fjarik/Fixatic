using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - Group service
	/// </summary>
	public interface IGroupsService
	{
		/// <summary>
		/// Creates the or update Group.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(Group entry);

		/// <summary>
		/// Gets all Groups.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Group>>> GetAllAsync();

		/// <summary>
		/// Gets the user groups.
		/// </summary>
		/// <returns></returns>
		Task<ServiceResponse<List<Group>>> GetUserGroupsAsync();

		/// <summary>
		/// Deletes the Group.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
