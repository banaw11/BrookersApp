using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories
{
    public class ConnectionRepository : IConnectionRepository
    {
        private readonly DataContext _context;
        public ConnectionRepository(DataContext context)
        {
            _context = context;
        }

        public void AddConnection(Connection connection)
        {
            _context.Connections.Add(connection);
        }

        public async void DeleteConnection(string connectionId)
        {
            _context.Connections.Remove(await GetConnection(connectionId));
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _context.Connections
                .Where(x => x.ConnectionId == connectionId)
                .FirstOrDefaultAsync();
        }

        public async Task<ICollection<string>> GetConnections(AppUser user)
        {
            return await _context.Connections
               .Where(x => x.User == user)
               .Select(x => x.ConnectionId)
               .ToListAsync();
        }

        public async Task<ICollection<string>> GetFriendConnectionIDs(int userId)
        {
            return await _context.Connections
               .Where(x => x.User.FriendsInvited.Select(x => x.UserId).Contains(userId) 
                    || x.User.FriendsAccepted.Select(x => x.FriendId).Contains(userId))
               .Select(x => x.ConnectionId)
               .ToListAsync();
        }

        public async Task<ICollection<int>> GetOnlineFriends(int userId)
        {
            return await _context.Connections
               .Where(x => x.User.FriendsInvited.Select(x => x.UserId).Contains(userId) 
                    || x.User.FriendsAccepted.Select(x => x.FriendId).Contains(userId))
               .Select(x => x.UserId)
               .Distinct()
               .ToListAsync();
        }
    }
}