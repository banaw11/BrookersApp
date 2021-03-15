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

        public async Task<NotificationDto> GetNotifications(int userId)
        {
            var notification = new NotificationDto();
            var unreadMessages = await _context.Messages
                .Where(x => x.ReceiverId == userId && x.IsRead == false)
                .Select(x => new UnreadMessageDto {MessageId = x.Id, SenderId = x.SenderId})
                .ToListAsync();
            notification.UnreadMessages = unreadMessages;
            return notification;
        }

    }
}
