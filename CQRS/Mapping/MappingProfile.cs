using AutoMapper;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.Models;

namespace JobPortal.CQRS.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, 
                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            
            // Map CreateUserDto to User entity
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.CreatedAt, 
                           opt => opt.Ignore()); // CreatedAt is set in the handler
            
            // Map UpdateUserDto to User entity, ignoring null values
            CreateMap<UpdateUserDto, User>()
                .ForAllMembers(opts => opts.Condition(
                    (src, dest, srcMember) => srcMember != null));
        }
    }
} 