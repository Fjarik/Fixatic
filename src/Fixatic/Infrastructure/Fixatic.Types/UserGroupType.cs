using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	[Flags]
	public enum UserGroupType
	{
		Internal = 2,
		External = 4,
		Admin = 8,
	}
}
