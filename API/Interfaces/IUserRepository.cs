using API.DTOs;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserByIdAsync(int userId);
        Task<ICollection<FriendDto>> GetFriends(int userId);
        Task<ICollection<MessageDto>> GetMessagesThread(int userId, int memberId);
        Task<ProfileDto> GetUserProfile(string userName, bool isOwner);
    }
}
