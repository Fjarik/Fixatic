using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class FullProperty : CustomProperty
	{
		public List<CustomPropertyOption> Options { get; set; } = new();
	}
}
