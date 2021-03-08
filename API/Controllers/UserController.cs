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

        
    }
}
