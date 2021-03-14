using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.SignalR._interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitOfHub _unitOfHub;
        public ChatController(IMapper mapper, IUnitOfWork unitOfWork, IUnitOfHub unitOfHub)
        {
            _unitOfHub = unitOfHub;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpPost("new-message")]
        public async Task<ActionResult<MessageDto>> NewMessage(NewMessageDto newMessageDto)
        {
            var sender = await _unitOfWork.UserRepository.GetUserByIdAsync(User.GetUserId());

            if (sender.Id == newMessageDto.ReceiverId)
                return BadRequest("You cannot send messages to yourself");

            var receiver = await _unitOfWork.UserRepository.GetUserByIdAsync(newMessageDto.ReceiverId);

            if (receiver == null)
                return NotFound();

            var message = new Message
            {
                Sender = sender,
                Receiver = receiver,
                Content = newMessageDto.Content,
                SendDate = DateTime.UtcNow,
                IsRead = false
            };

            _unitOfWork.MessageRepository.AddMessage(message);
            if (_unitOfWork.hasChanges())
                if (await _unitOfWork.Complete())
                {
                    var messageDto = _mapper.Map<MessageDto>(message);
                    if(await NotifyNewMessage(message, receiver))
                        return Ok(messageDto);
                }
                    
            return BadRequest("Failed to send message");
        }

        [HttpGet("{memberId}", Name = "GetMessages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int memberId)
        {
            var messages = await _unitOfWork.UserRepository.GetMessagesThread(User.GetUserId(), memberId);
            return Ok(messages);
        }

        private async Task<bool> NotifyNewMessage(Message message, AppUser user)
        {
            var result = await _unitOfHub.MessageService.SendMessage(message, user);
            if (result) return result;
           
            _unitOfWork.NotificationRepository.CreateUnreadMessageNotification(message, user);
            if (_unitOfWork.hasChanges())
                if (await _unitOfWork.Complete())
                    return !result;

            return result;

           
        }
    }
}