using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class CurrentUser : User
	{
		public UserGroupType GroupType { get;set;}

		public bool IsInGroup(UserGroupType type)
		{
			return GroupType.HasFlag(type);
		}
	}
}
