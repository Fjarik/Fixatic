using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class Follower
    {
        public int UserId { get; set; }

        public int TicketId { get; set; }

        public DateTime Since { get; set; }

        public int Type { get; set; }

    }
}
