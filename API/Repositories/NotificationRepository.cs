using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public NotificationRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<FriendDto>> GetInvitations(int userId)
        {
            var friendsNotConfirmed = await _context.Users.Where(x => x.Id == userId)
                .Select(x => x.FriendsAccepted).FirstOrDefaultAsync();
            var notConfirmedIds = friendsNotConfirmed.Where(x => x.IsConfirmed == false).Select(x => x.FriendId).ToList();

            return await _context.Users.Where(x => notConfirmedIds.Contains(x.Id))
                .Select(x => new FriendDto { FriendName = x.UserName, FriendId = x.Id, Avatar = x.Avatar }).ToListAsync();
        }

        public async Task<NotificationDto> GetNotifications(int userId)
        {
            var notification = new NotificationDto();

            var unreadMessages = await _context.Messages
                .Where(x => x.ReceiverId == userId && x.IsRead == false)
                .Select(x => new UnreadMessageDto {MessageId = x.Id, SenderId = x.SenderId})
                .ToListAsync();

            notification.Invitations = await GetInvitations(userId);
            notification.UnreadMessages = unreadMessages;
           
            return notification;
        }

    }
}
