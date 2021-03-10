using API.Entities;
using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace API.SignalR
{
    public class PressenceHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public PressenceHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var isOnline = await IsUserOnilne();
            if(isOnline) await Clients.Caller.SendAsync("GetOnilneFriends", 
                await _unitOfWork.UserRepository.GetOnlineFriends(Context.User.GetUserId()));
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOfline = await UserDisconnected(Context.User.GetUserId(), Context.ConnectionId);
            if(isOfline) await Clients.Others.SendAsync("FriendIsOfline", Context.User.GetUserId());
            await base.OnDisconnectedAsync(exception);
        }

        private Connection CreateConnection(AppUser user)
        {
            return new Connection { ConnectionId = Context.ConnectionId, User = user };
        }

        private async Task<bool> UserConnected(int userId, string connectionId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);
            var connection = CreateConnection(user);
            
            if (user != null) _unitOfWork.UserRepository.AddConnection(connection);
            if(_unitOfWork.hasChanges())
                return await Task.FromResult(await _unitOfWork.Complete());
            
            return false;
        }

        private async Task<bool> IsUserOnilne()
        {
            var isOnline = await UserConnected(Context.User.GetUserId(), Context.ConnectionId);
            if(isOnline) 
            {
                await Clients.Others.SendAsync("FriendIsOnline",Context.User.GetUserId());
                return true;
            }
            return false;
        }

        private async Task<bool> UserDisconnected(int userId, string connectionId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if(user != null) _unitOfWork.UserRepository.DeleteConnection(connectionId);
            if(_unitOfWork.hasChanges())
                return await Task.FromResult(await _unitOfWork.Complete());   
    
            return false;
        }
    }
}
