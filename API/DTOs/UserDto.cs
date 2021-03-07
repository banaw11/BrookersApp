using System.Collections.Generic;

namespace API.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public ICollection<FriendDto> Friends { get; set; }
        
    }
}
