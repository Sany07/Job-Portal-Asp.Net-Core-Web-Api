using JobPortal.CQRS.Common.Models;
using JobPortal.CQRS.Users.DTOs;
using MediatR;

namespace JobPortal.CQRS.Users.Queries.GetUsers
{
    public class GetUsersQuery : PagedQuery, IRequest<Result<JobPortal.CQRS.Common.Models.PagedResult<UserDto>>>
    {
        // Inherits properties from PagedQuery:
        // - PageNumber
        // - PageSize
        // - SearchTerm
        // - SortColumn
        // - SortDirection
    }
} 