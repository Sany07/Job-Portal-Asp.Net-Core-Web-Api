
using JobPortal.CQRS.Users.DTOs;
using MediatR;
using JobPortal.CQRS.Common.Models;

namespace JobPortal.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Result<int>>
    {
        public CreateUserDto UserDto { get; }

        public CreateUserCommand(CreateUserDto userDto)
        {
            UserDto = userDto;
        }
    }
} 