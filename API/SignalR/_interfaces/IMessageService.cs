using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.SignalR._interfaces
{
    public interface IMessageService
    {
        Task AddMessage(MessageDto message, AppUser sedner);
        Task SendMessage(Message message, AppUser receiver, AppUser sender);
    }
}