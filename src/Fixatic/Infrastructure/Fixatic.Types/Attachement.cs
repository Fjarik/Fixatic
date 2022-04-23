using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fixatic.Types
{
    public class Attachement
    {
        public int AttachementId { get; set; }

        public int UserId { get; set; }

        public int TicketId { get; set; }

        public byte[] Content { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public string Type { get; set; }

        public DateTime Uploaded { get; set; }

        public string? AlternativeText { get; set; }

    }
}
