using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NewMessageDto
    {
        public int ReceiverId { get; set; }
        public string Content { get; set; }
    }
}
