using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using API.SignalR._interfaces;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR._repositories
{
    public class MessageService : IMessageService
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        public MessageService(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        public async Task<bool> SendMessage(MessageDto message, AppUser user)
        {
            var connectionIDs = await _unitOfWork.ConnectionRepository.GetConnections(user);

           if(connectionIDs.Any()) 
           {
               await _hubContext.Clients.Clients(connectionIDs).SendAsync("GetNewMessage", message);
               return true;
           }
            return false;

        }
    }
}