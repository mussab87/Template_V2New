using AutoMapper;

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

        // Map from RoleDto to Role
        CreateMap<RoleDto, Role>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            // Map any other properties that need special handling
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

        // Map from Role to RoleDto
        CreateMap<Role, RoleDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
    }
}
