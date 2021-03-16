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
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{userName}", Name = "GetProfile")]
        public async Task<ActionResult<ProfileDto>> GetProfile(string userName)
        {
            var currentUser = User.GetUserName();

            return await _unitOfWork.UserRepository.GetUserProfile(userName, isOwner: currentUser.ToLower() == userName.ToLower(), User.GetUserId());
        }
    }
}
