using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class User
    {
        public int UserId { get; set; }

        public DateTime Created { get; set; }

        public bool IsEnabled { get; set; }

        public string Email { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

    }
}
