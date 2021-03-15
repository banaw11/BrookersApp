using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class UnreadMessageDto
    {
        public int MessageId { get; set; }
        public int SenderId { get; set; }
    }
}
