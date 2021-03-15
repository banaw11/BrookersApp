using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IMessageRepository
    {
        void AddMessage(Message message);
        Task<ICollection<Message>> GetMessages(int currentUserId, int otherUserId);
        Task<Message> MarkAsRead(int messageId);
    }
}
