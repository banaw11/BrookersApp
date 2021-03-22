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

        public async void AcceptInvite(int contextUserId, int userId)
        {
            var relation = await _context.Friends.FindAsync(userId, contextUserId);
            if(relation != null)
            {
                relation.IsConfirmed = true;
                _context.Entry(relation).State = EntityState.Modified;
            }
            
        }

        public  void AddFriend(int friendId, int userId)
        {
            _context.Friends.Add(new Friend { FriendId = friendId, UserId = userId, IsConfirmed = false });
        }

        public async void DeclineInvite(int contextUserId, int userId)
        {
            var relation = await _context.Friends.FindAsync(contextUserId, userId);
            _context.Friends.Remove(relation);
        }

        public async void DeleteFirend(int userId, int friendId)
        {
            var friend = await _context.Friends.FindAsync( userId, friendId );
            if(friend == null)
                friend = await _context.Friends.FindAsync(friendId, userId );

            _context.Friends.Remove(friend);
        }

        public async Task<ICollection<int>> GetFriendIds(int userId)
        {
             var result = await _context.Users
                .Where(x => x.Id == userId)
                .Select(x => new {x.FriendsAccepted, x.FriendsInvited })
                .FirstOrDefaultAsync();
            return  result.FriendsAccepted
                .Where(x => x.IsConfirmed == true)
                .Select(x => x.UserId).ToList()
                .Concat(result.FriendsInvited.Where(x => x.IsConfirmed == true).Select(x => x.FriendId)).ToList();
        }

        public async Task<ICollection<FriendDto>> GetFriends(int userId)
        {
            var friendIds = await GetFriendIds(userId);
            return await _context.Users
                .Where(x =>  friendIds.Contains(x.Id))
                .Select(x => new FriendDto {FriendId = x.Id, FriendName = x.UserName, Avatar = x.Avatar})
                .ToListAsync();
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

        public async Task<ProfileDto> GetUserProfile(string userName, bool isOwner, int callerId)
        {
            var profile = await _context.Users
                 .Where(x => x.NormalizedUserName == userName.ToUpper())
                 .Select(x => new ProfileDto { UserName = x.UserName, UserId = x.Id, Image = x.Avatar, IsOwner= isOwner})
                 .FirstOrDefaultAsync();

            if (!isOwner)
            {
                profile.IsFriend = await IsFriend(callerId, profile.UserId);
                profile.IsInvited = await IsInvited(callerId, profile.UserId);
            }
            else
            {
                profile.IsFriend = false;
                profile.IsInvited = false;
            }
            
            
            return profile;
        }

        public async Task<bool> IsFriend(int contextUserId, int userId)
        {
            var friendIds = await GetFriendIds(contextUserId);

            if (friendIds.Contains(userId))
                return true;

            return false;
        }

        public async Task<bool> IsInvited(int contextId, int userId)
        {
            var relation = await _context.Friends.FindAsync(contextId, userId);
            if (relation == null)
                return false;
            return !relation.IsConfirmed;
        }
    }
}
