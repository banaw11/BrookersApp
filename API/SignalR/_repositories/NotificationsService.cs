using API.SignalR._interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR._repositories
{
    public class NotificationsService : INotificationsService
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        public NotificationsService(IHubContext<PressenceHub> hubContext)
        {
            _hubContext = hubContext;
        }
    }
}