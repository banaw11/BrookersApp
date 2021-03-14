using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class UnreadMessage
    {
        public int Id { get; set; }
        public int MessageId { get; set; }
        public int SenderId { get; set; }
        public Notification Notification { get; set; }
        public int NotificationId { get; set; }
    }
}
