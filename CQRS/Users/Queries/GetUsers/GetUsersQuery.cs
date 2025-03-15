using JobPortal.CQRS.Common;
using JobPortal.CQRS.Users.DTOs;
using MediatR;

namespace JobPortal.CQRS.Users.Queries.GetUsers
{
    public class GetUsersQuery : IRequest<PagedResult<UserDto>>
    {
        public string SearchTerm { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
} 