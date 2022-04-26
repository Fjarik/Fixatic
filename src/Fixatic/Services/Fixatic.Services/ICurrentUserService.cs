using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.Types;

namespace Fixatic.Services
{
	/// <summary>
	/// Interface - current user service
	/// </summary>
	public interface ICurrentUserService
	{
		/// <summary>
		/// Gets the user information.
		/// </summary>
		/// <returns></returns>
		Task<CurrentUser> GetUserInfoAsync();

		/// <summary>
		/// Determines whether user is logged in.
		/// </summary>
		/// <returns></returns>
		Task<bool> IsLoggedInAsync();

		/// <summary>
		/// Invalidates the cache.
		/// </summary>
		void InvalidateCache();
	}
}
