using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ChatController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        public ChatController(IMapper mapper, IUnitOfWork unitOfWork)
        {
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
            if(_unitOfWork.hasChanges())
                if(await _unitOfWork.Complete())    
                    return Ok(_mapper.Map<MessageDto>(message));
                    
            return BadRequest("Failed to send message");
        }

        [HttpGet("{memberId}", Name = "GetMessages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int memberId)
        {
            var messages = await _unitOfWork.UserRepository.GetMessagesThread(User.GetUserId(), memberId);
            return Ok(messages);
        }
    }
}