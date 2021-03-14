using API.Interfaces;
using API.SignalR._interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR._repositories
{
    public class UnitOfHub : IUnitOfHub
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UnitOfHub(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork, IMapper mapper )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public IMessageService MessageService => new MessageService(_hubContext, _unitOfWork, _mapper);
        public INotificationsService NotificationsService => new NotificationsService(_hubContext);
    }
}