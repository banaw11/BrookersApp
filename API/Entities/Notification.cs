using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Notification
    {
        public Notification()
        {

        }
        public Notification(AppUser user)
        {
            UserId = user.Id;
            User = user;
            UnreadMessages = new List<UnreadMessage>();
        }
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; }
        public ICollection<UnreadMessage> UnreadMessages { get; set; }
    }
}
