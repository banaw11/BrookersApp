using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class ProfileDto
    {
        public string UserName { get; set; }
        public int UserId { get; set; }
        public string Image { get; set; }
        public bool IsOwner { get; set; }
        public bool IsFriend { get; set; }
    }
}
