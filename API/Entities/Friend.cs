using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("Friends")]
    public class Friend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public  AppUser User { get; set; }
        public int FriendId { get; set; }
        public string FriendName { get; set; }
        public string Avatar { get; set; }
    }
}
