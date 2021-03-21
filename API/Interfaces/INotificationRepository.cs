using API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface INotificationRepository
    {
        Task<NotificationDto> GetNotifications (int userId);
        Task<ICollection<FriendDto>> GetInvitations(int userId);
    }
}
