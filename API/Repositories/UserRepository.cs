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

        public async Task<AppUser> GetUserByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<ProfileDto> GetUserProfile(string userName, bool isOwner)
        {
            return await _context.Users
                 .Where(x => x.NormalizedUserName == userName.ToUpper())
                 .Select(x => new ProfileDto { UserName = x.UserName, UserId = x.Id, Image = x.Avatar, IsOwner = isOwner })
                 .FirstOrDefaultAsync();
        }
    }
}
