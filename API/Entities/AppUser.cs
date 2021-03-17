using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public float Balance { get; set; }
        public string Avatar { get; set; }
        public ICollection<AppUserRole> UserRoles { get; set; }
        public virtual ICollection<Friend> FriendsInvited { get; set; }
        public virtual ICollection<Friend> FriendsAccepted { get; set; }
        public ICollection<Message> MessagesReceived { get; set; }
        public ICollection<Message> MessagesSent { get; set; }
        public ICollection<Connection> Connections { get; set; }
    }
}
