using API.Interfaces;
using API.SignalR._interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR._repositories
{
    public class UnitOfHub : IUnitOfHub
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        public UnitOfHub(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public IMessageService MessageService => new MessageService(_hubContext, _unitOfWork);
        public INotificationsService NotificationsService => new NotificationsService(_hubContext);
    }
}