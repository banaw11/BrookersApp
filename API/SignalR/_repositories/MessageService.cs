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

        public async Task AddMessage(MessageDto message, AppUser sender)
        {
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(sender);
            if(connectionIDs.Any())
                await _hubContext.Clients.Clients(connectionIDs).SendAsync("AddMessage", message);
        }

        public async Task SendMessage(Message message, AppUser receiver, AppUser sender)
        {
            var messageDto = _mapper.Map<MessageDto>(message);
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(receiver);
            await AddMessage(messageDto, sender);
           if(connectionIDs.Any()) 
               await _hubContext.Clients.Clients(connectionIDs).SendAsync("GetNewMessage", messageDto);

        }

        
    }
}