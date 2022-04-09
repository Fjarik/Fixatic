using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class ProjectCategory
	{
		public int CategoryId { get; set; }

		public string Description { get; set; }

		public string Name { get; set; }

		// Note: this is important for MudSelect
		public override bool Equals(object? o)
		{
			var other = o as ProjectCategory;
			return other?.Name == Name;
		}

		// Note: this is important for MudSelect
		public override int GetHashCode()
		{
			return Name?.GetHashCode() ?? 0;
		}

		// Note: this is important for MudSelect
		public override string ToString()
		{
			return Name;
		}
	}
}