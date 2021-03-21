

using API.DTOs;
using System.Threading.Tasks;

namespace API.SignalR._interfaces
{
    public interface IInviteService
    {
        Task SendInvite(int contextUser, int userId);
        Task SendInviteAcceptInfo(int user, int friendId);
    }
}
