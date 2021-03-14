using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.SignalR._interfaces;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR._repositories
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MessageService(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<bool> SendMessage(Message message, AppUser user)
        {
            var messageDto = _mapper.Map<MessageDto>(message);
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(user);

           if(connectionIDs.Any()) 
           {
               await _hubContext.Clients.Clients(connectionIDs).SendAsync("GetNewMessage", messageDto);
               return true;
           }
            return false;

        }
    }
}