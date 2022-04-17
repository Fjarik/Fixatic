using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class FullTicketProperty
	{
		public CustomPropertyOption PropertyOption { get; set; }

		public bool IsSelected { get; set; }

		public FullTicketProperty(CustomPropertyOption propertyOption, bool isSelected)
		{
			PropertyOption = propertyOption;
			IsSelected = isSelected;
		}
	}
}
