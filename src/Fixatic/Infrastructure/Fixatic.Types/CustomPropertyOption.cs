using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class CustomPropertyOption
    {
        public int CustomPropertyOptionId { get; set; }

        public int CustomPropertyId { get; set; }

        public string Content { get; set; }

        public bool IsEnabled { get; set; }

        public int Sequence { get; set; }
		
		public bool CanDelete { get; set; } = false;
	}
}
