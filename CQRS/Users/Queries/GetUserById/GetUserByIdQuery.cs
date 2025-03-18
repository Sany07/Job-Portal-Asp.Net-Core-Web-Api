using JobPortal.CQRS.Common.Models;
using JobPortal.CQRS.Users.DTOs;
using MediatR;

namespace JobPortal.CQRS.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<Result<UserDto>>
    {
        public int UserId { get; }

        public GetUserByIdQuery(int userId)
        {
            UserId = userId;
        }
    }
} 