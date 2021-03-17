using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Connection
    {
        public string ConnectionId { get; set; }
        public AppUser User { get; set; }
        public int UserId { get; set; }
    }
}
