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
            var onlineFriends = await _unitOfWork.ConnectionRepository.GetOnlineFriends(Context.User.GetUserId());
             await Clients.Caller.SendAsync("GetOnlineFriends", onlineFriends);
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var isOfline = await UserDisconnected(Context.User.GetUserId(), Context.ConnectionId);
            var friendsConnection = await _unitOfWork.ConnectionRepository.GetFriendConnectionIDs(Context.User.GetUserId());
            if(isOfline) await Clients.Clients(friendsConnection).SendAsync("FriendIsOfline", Context.User.GetUserId());
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
            
            if (user != null) _unitOfWork.ConnectionRepository.AddConnection(connection);
            if(_unitOfWork.hasChanges())
                return await Task.FromResult(await _unitOfWork.Complete());
            
            return false;
        }

        private async Task<bool> IsUserOnilne()
        {
            var isOnline = await UserConnected(Context.User.GetUserId(), Context.ConnectionId);
            if(isOnline) 
            {
                var friendsConnection = await _unitOfWork.ConnectionRepository.GetFriendConnectionIDs(Context.User.GetUserId());
                await Clients.Clients(friendsConnection).SendAsync("FriendIsOnline",Context.User.GetUserId());
                return true;
            }
            return false;
        }

        private async Task<bool> UserDisconnected(int userId, string connectionId)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(userId);

            if(user != null) _unitOfWork.ConnectionRepository.DeleteConnection(connectionId);
            if(_unitOfWork.hasChanges())
                return await Task.FromResult(await _unitOfWork.Complete());   
    
            return false;
        }
    }
}
