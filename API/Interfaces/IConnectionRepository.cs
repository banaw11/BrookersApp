using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;

namespace API.Interfaces
{
    public interface IConnectionRepository
    {
        void AddConnection (Connection connection);
        Task<ICollection<Connection>> GetConnections(AppUser user);
        Task<Connection> GetConnection(string connectionId);
        void DeleteConnection (string connectionId);
        Task<ICollection<int>> GetOnlineFriends(int userId);
        Task<ICollection<string>> GetFriendConnectionIDs (int userId);
    }
}