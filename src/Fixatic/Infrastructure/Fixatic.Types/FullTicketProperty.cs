using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
	public class FullTicketProperty
	{
		public CustomProperty Value { get; set; }

		public List<FullTicketPropertyOption> Options { get; set; } = new();

		public FullTicketProperty(CustomProperty value, List<FullTicketPropertyOption> options)
		{
			Value = value;
			Options = options;
		}
	}

	public class FullTicketPropertyOption
	{
		public CustomPropertyOption Value { get; set; }

		public bool IsSelected { get; set; }

		public FullTicketPropertyOption(CustomPropertyOption propertyOption, bool isSelected)
		{
			Value = propertyOption;
			IsSelected = isSelected;
		}
	}

}
