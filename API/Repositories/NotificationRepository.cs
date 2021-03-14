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

        public async void CreateUnreadMessageNotification(Message message, AppUser user)
        {
            var notification = await _context.Users.Where(x => x.Id == user.Id).Select(x => x.Notification).SingleOrDefaultAsync();
            var unreadMessage = new UnreadMessage
            {
                MessageId = message.Id,
                SenderId = message.SenderId,
                Notification = notification
            };

             _context.UnreadMessages.Add(unreadMessage);
        }


        public async void RemoveUnreadMessageNotification(int messageId)
        {
            var unreadMessage = await _context.UnreadMessages
                .Where(x => x.MessageId == messageId).FirstOrDefaultAsync();
            _context.UnreadMessages.Remove(unreadMessage);
        }
    }
}
