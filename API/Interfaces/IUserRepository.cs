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
        Task<ICollection<int>> GetFriendIds (int userId);
        Task<ICollection<MessageDto>> GetMessagesThread(int userId, int memberId);
        Task<ProfileDto> GetUserProfile(string userName, bool isOwner, int callerId);
        void AddFriend(int userId, int friendId);
        Task<bool> IsFriend(int contextUserId, int userId);
        void AcceptInvite(int contextUserId, int userId);
        void DeclineInvite(int contextUserId, int userId);
        void DeleteFirend(int userId, int friendId);
    }
}
