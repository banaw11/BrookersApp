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
        void AddConnection (Connection connection);
        Task<ICollection<Connection>> GetConnections(AppUser user);
        Task<Connection> GetConnection(string connectionId);
        void DeleteConnection (string connectionId);
        Task<ICollection<int>> GetOnlineFriends(int userId);
        Task<bool> SaveChangesAsync();

    }
}
