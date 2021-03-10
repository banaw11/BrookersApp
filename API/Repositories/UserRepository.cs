using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public async Task<ICollection<Connection>> GetConnections(AppUser user)
        {
            return await _context.Connections
                .Where(x => x.User == user)
                .ToListAsync();
        }

        public async Task<ICollection<FriendDto>> GetFriends(int userId)
        {
            var result = await _context.Users.Where(x => x.Id == userId).Select(x => x.Friends).FirstOrDefaultAsync();
            return _mapper.Map<ICollection<FriendDto>>(result);
        }

        public async Task<ICollection<MessageDto>> GetMessagesThread(int userId, int memberId)
        {
            var messagesReceived = await _context.Users.Where(x => x.Id == userId).Select(x => x.MessagesReceived).FirstOrDefaultAsync();
            var messagesSent = await _context.Users.Where(x => x.Id == userId).Select(x => x.MessagesSent).FirstOrDefaultAsync();
            var messages = Enumerable.Concat(
                messagesReceived.Where(x => x.SenderId == memberId),
                messagesSent.Where(x => x.ReceiverId == memberId)
                ).OrderBy(x => x.SendDate).ToList();

            return _mapper.Map<ICollection<MessageDto>>(messages);
        }

        public async Task<ICollection<int>> GetOnlineFriends(int userId)
        {
            return await _context.Connections
                .Where(x => x.UserId != userId && x.User.Friends.Select(x => x.FriendId).Contains(userId))
                .GroupBy(x => x.User)
                .Select(x => x.FirstOrDefault())
                .Select(x => x.UserId)
                .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
