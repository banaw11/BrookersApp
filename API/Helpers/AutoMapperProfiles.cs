using API.DTOs;
using API.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<RegisterDto, AppUser>();
            CreateMap<Friend, FriendDto>()
                .ReverseMap();
            CreateMap<AppUser, UserDto>()
                .ReverseMap();
            CreateMap<Message, MessageDto>()
                .ReverseMap();
            CreateMap<Notification, NotificationDto>()
                .ReverseMap();
            CreateMap<UnreadMessage, UnreadMessageDto>()
                .ReverseMap();


        }
    }
}
