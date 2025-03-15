using JobPortal.CQRS.Common;
using JobPortal.CQRS.Users.DTOs;
using MediatR;

namespace JobPortal.CQRS.Users.Commands.UpdateUser
{
    public class UserUpdateCommand : IRequest<Result>
    {
        public int Id { get; }
        public UpdateUserDto UserDto { get; }

        public UserUpdateCommand(int id, UpdateUserDto userDto)
        {
            Id = id;
            UserDto = userDto;
        }
    }
} 