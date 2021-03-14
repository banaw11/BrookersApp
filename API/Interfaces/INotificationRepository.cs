using API.DTOs;
using API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Interfaces
{
   public interface INotificationRepository
    {
        void RemoveUnreadMessageNotification(int messageId);
        void CreateUnreadMessageNotification(Message message, AppUser user);
    }
}
