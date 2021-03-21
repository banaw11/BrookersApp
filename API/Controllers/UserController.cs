using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using API.SignalR._interfaces;
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
        private readonly IUnitOfHub _unitOfHub;

        public UserController(IMapper mapper, IUnitOfWork unitOfWork, IUnitOfHub unitOfHub)
        {
            _unitOfWork = unitOfWork;
            _unitOfHub = unitOfHub;
            _mapper = mapper;
        }

        [HttpGet("{userName}", Name = "GetProfile")]
        public async Task<ActionResult<ProfileDto>> GetProfile(string userName)
        {
            var currentUser = User.GetUserName();

            return await _unitOfWork.UserRepository.GetUserProfile(userName, isOwner: currentUser.ToLower() == userName.ToLower(), User.GetUserId());
        }

        [HttpPost("send-invite/{userId}", Name = "SendInvite")]
        public async Task<ActionResult> SendInvite(int userId)
        {
            var contextUserId = User.GetUserId();
            if (await _unitOfWork.UserRepository.GetUserByIdAsync(userId) == null) return BadRequest("User not found");
            if (await _unitOfWork.UserRepository.IsFriend(contextUserId, userId)) return BadRequest("User is already added to friends");

            _unitOfWork.UserRepository.AddFriend(contextUserId, userId);
            if (_unitOfWork.hasChanges())
                if (await _unitOfWork.Complete())
                {
                    await _unitOfHub.InviteService.SendInvite(contextUserId, userId);
                    return Ok();
                }

            return BadRequest("Couldn't send invite to friends");
        }

        [HttpPost("accept-invite/{userId}")]
        public async Task<ActionResult> ConfirmInvite(int userId, [FromBody]bool isAcepted)
        {
            var contextUserId = User.GetUserId();
            if (await _unitOfWork.UserRepository.GetUserByIdAsync(userId) == null) return BadRequest("User not found");

            if (isAcepted)
                _unitOfWork.UserRepository.AcceptInvite(contextUserId, userId);
            else
                _unitOfWork.UserRepository.DeclineInvite(contextUserId, userId);

            if (_unitOfWork.hasChanges())
                if (await _unitOfWork.Complete())
                {
                    if (isAcepted)
                    {
                        await _unitOfHub.InviteService.SendInviteAcceptInfo(userId, contextUserId);
                        await _unitOfHub.NotificationsService.RefreshFriends(userId);
                        await _unitOfHub.NotificationsService.RefreshFriends(contextUserId);
                    }
                    await _unitOfHub.NotificationsService.RefreshInvitations(contextUserId);
                    return Ok();
                }

            return BadRequest("Couldn't answer on invite");
        }

        [HttpPost("delete-friend/{userId}")]
        public async Task<ActionResult> DeleteFriend(int userId)
        {
            var contextUserId = User.GetUserId();

            if (await _unitOfWork.UserRepository.IsFriend(userId, contextUserId) == false &&
                await _unitOfWork.UserRepository.IsFriend(contextUserId, userId) == false)
                return BadRequest("User is not friend");

            _unitOfWork.UserRepository.DeleteFirend(contextUserId, userId);

            if(_unitOfWork.hasChanges())
                if(await _unitOfWork.Complete())
                {
                    await _unitOfHub.NotificationsService.RefreshFriends(userId);
                    await _unitOfHub.NotificationsService.RefreshFriends(contextUserId);
                    return Ok();
                }

            return BadRequest("Couldn't delete user from friends");
        }
    }
}
