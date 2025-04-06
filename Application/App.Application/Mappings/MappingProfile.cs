﻿using AutoMapper;

namespace App.Application.Mappings { }

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Map from UserDto to User
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username))
            // Map any other properties that need special handling
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        // Map from User to UserDto
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName));
    }
}
