using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fixatic.Types;

namespace Fixatic.Services
{
	public interface ICurrentUserService
	{
		Task<CurrentUser> GetUserInfoAsync();

		void InvalidateCache();
	}
}
