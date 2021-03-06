﻿using API.Data;
using API.Entities;
using API.Interfaces;
using API.SignalR._interfaces;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;

        public MessageRepository(DataContext context)
        {
            _context = context;
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public async Task<ICollection<Message>> GetMessages(int currentUserId, int otherUserId)
        {
            return await _context.Messages
                .Where(m => m.SenderId == currentUserId
                        && m.ReceiverId == otherUserId
                        || m.SenderId == otherUserId
                        && m.ReceiverId == currentUserId)
                .OrderBy(m => m.SendDate)
                .ToListAsync();
        }

        public async Task<Message> MarkAsRead(int messageId)
        {
            var message = await _context.Messages.FindAsync(messageId);
            message.IsRead = true;
            return message;
        }
    }
}
