using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;

namespace API.SignalR._interfaces
{
    public interface IMessageService
    {
        Task<bool> SendMessage(Message message, AppUser user);
    }
}