using API.Interfaces;
using API.SignalR._interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR._repositories
{
    public class NotificationsService : INotificationsService
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public NotificationsService(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        public async Task RefreshFriends(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(user);
            var friendList = await _unitOfWork.UserRepository.GetFriends(userId);

            if (connectionIDs.Any())
                await _hubContext.Clients.Clients(connectionIDs).SendAsync("RefreshedFriendsList", friendList);
        }

        public async Task RefreshInvitations(int userId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(user);
            var invitations = await _unitOfWork.NotificationRepository.GetInvitations(userId);

            if(connectionIDs.Any())
                await _hubContext.Clients.Clients(connectionIDs).SendAsync("InvitationsRefreshed", invitations);
        }
    }
}