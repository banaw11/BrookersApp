using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IMessageRepository _messageRepository;

        public UserController(IMapper mapper, IUserRepository userRepository, IMessageRepository messageRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost("new-message")]
        public async Task<ActionResult<MessageDto>> NewMessage(NewMessageDto newMessageDto)
        {
            var sender = await _userRepository.GetUserByIdAsync(User.GetUserId());

            if (sender.Id == newMessageDto.ReceiverId)
                return BadRequest("You cannot send messages to yourself");

            var receiver = await _userRepository.GetUserByIdAsync(newMessageDto.ReceiverId);

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

            _messageRepository.AddMessage(message);

            if (await _messageRepository.SaveChangesAsync())
                return Ok(_mapper.Map<MessageDto>(message));

            return BadRequest("Failed to send message");

        }

        [HttpGet("{memberId}", Name ="GetMessages")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessages(int memberId)
        {
            var messages = await _userRepository.GetMessagesThread(User.GetUserId(), memberId);

            return Ok(messages);
        }
    }
}
