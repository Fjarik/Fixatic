using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class ApplicationSettings
    {
        public string ConnectionString { get; set; } = string.Empty;

        public Version AppVersion { get; set; } = new Version(1,0,0);

        public string BasePath { get; set; } = "/";
    }
}
