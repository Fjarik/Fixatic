using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class Project
    {
        public int ProjectId { get; set; }

        public string Description { get; set; }

        public bool IsEnabled { get; set; }

        public bool IsInternal { get; set; }

        public string Name { get; set; }

        public string Shortcut { get; set; }

    }
}
