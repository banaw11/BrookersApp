using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Connection
    {
        public int Id { get; set; }
        public int ConnectionId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
