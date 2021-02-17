using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

         [HttpPost("register")]
         public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserNameExists(registerDto.UserName)) return BadRequest("User name is taken");
            if (await UserEmailExists(registerDto.Email)) return BadRequest("Address email already exist");

            var user = _mapper.Map<AppUser>(registerDto);

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return new UserDto
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        private async Task<bool> UserNameExists(string username)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == username);
        }

        private async Task<bool> UserEmailExists(string email)
        {
            return await _userManager.Users.AnyAsync(x => x.Email == email);
        }
    }
}
