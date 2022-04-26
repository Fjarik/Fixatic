using Fixatic.Services.Types;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - CustomPropertyOption service
	/// </summary>
	public interface ICustomPropertyOptionsService
	{
		/// <summary>
		/// Creates the or update CustomPropertyOption.
		/// </summary>
		/// <param name="entry">The entry.</param>
		/// <returns></returns>
		Task<ServiceResponse<int>> CreateOrUpdateAsync(CustomPropertyOption entry);

		/// <summary>
		/// Gets all CustomPropertyOptions.
		/// </summary>
		/// <param name="propertyId">The property identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<List<CustomPropertyOption>>> GetAllAsync(int propertyId);

		/// <summary>
		/// Deletes the CustomPropertyOption.
		/// </summary>
		/// <param name="id">The identifier.</param>
		/// <returns></returns>
		Task<ServiceResponse<bool>> DeleteAsync(int id);
	}
}
