using API.DTOs;
using API.Interfaces;
using API.SignalR._interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.SignalR._repositories
{
    public class InviteService : IInviteService
    {
        private readonly IHubContext<PressenceHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;

        public InviteService(IHubContext<PressenceHub> hubContext, IUnitOfWork unitOfWork)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
        }

        public async Task SendInvite(int contextUserId, int userId)
        {
            var contextUser = await _unitOfWork.UserRepository.GetUserByIdAsync(contextUserId);
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            if(user != null)
            {
                var connectionsId = await _unitOfWork.ConnectionRepository.GetConnections(user);
                if (connectionsId.Any())
                   await _hubContext.Clients.Clients(connectionsId).SendAsync("GetNewInvite", new FriendDto
                                        {FriendId= contextUser.Id, FriendName = contextUser.UserName, Avatar = contextUser.Avatar });
            }
        }

        public async Task SendInviteAcceptInfo(int userId, int friendId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var connectionsIds = await _unitOfWork.ConnectionRepository.GetConnections(user);

            var friend = await _unitOfWork.UserRepository.GetUserByIdAsync(friendId);

            if(connectionsIds.Any())
             await _hubContext.Clients.Clients(connectionsIds).SendAsync("InviteAccepted",
                 new FriendDto {FriendId=friend.Id, FriendName= friend.UserName, Avatar=friend.Avatar });
        }
    }
}
