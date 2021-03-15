using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class NotificationDto
    {
        public ICollection<UnreadMessageDto> UnreadMessages { get; set; }
    }
}
