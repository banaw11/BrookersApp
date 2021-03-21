using System.Threading.Tasks;

namespace API.SignalR._interfaces
{
    public interface INotificationsService
    {
        Task RefreshInvitations(int userId);
        Task RefreshFriends(int userId);
    }
}