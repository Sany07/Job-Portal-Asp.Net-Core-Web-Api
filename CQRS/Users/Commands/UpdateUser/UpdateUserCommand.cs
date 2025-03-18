using JobPortal.CQRS.Common.Models;
using JobPortal.CQRS.Users.DTOs;
using MediatR;

namespace JobPortal.CQRS.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<Result<bool>>
    {
        public int UserId { get; }
        public UpdateUserDto UserDto { get; }

        public UpdateUserCommand(int userId, UpdateUserDto userDto)
        {
            UserId = userId;
            UserDto = userDto;
        }
    }
} 