using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common.Models;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, Result<JobPortal.CQRS.Common.Models.PagedResult<UserDto>>>
    {
        private readonly JobPortalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(JobPortalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<JobPortal.CQRS.Common.Models.PagedResult<UserDto>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Start with all users as IQueryable (not executed yet)
                var usersQuery = _dbContext.Users.AsNoTracking();

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    usersQuery = usersQuery.Where(u => 
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm) ||
                        u.FirstName.ToLower().Contains(searchTerm) ||
                        u.LastName.ToLower().Contains(searchTerm)
                    );
                }

                // Get total count before pagination
                var totalCount = await usersQuery.CountAsync(cancellationToken);

                // Apply sorting if provided, default to creation date descending
                var sortColumn = string.IsNullOrEmpty(request.SortColumn) ? "CreatedAt" : request.SortColumn;
                var sortDirection = string.IsNullOrEmpty(request.SortDirection) ? "desc" : request.SortDirection;
                
                usersQuery = usersQuery.OrderBy($"{sortColumn} {sortDirection}");

                // Apply pagination
                var pagedUsers = await usersQuery
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                // Map users to DTOs
                var userDtos = _mapper.Map<UserDto[]>(pagedUsers);

                // Create paged result
                var pagedResult = new JobPortal.CQRS.Common.Models.PagedResult<UserDto>(
                    userDtos,
                    totalCount,
                    request.PageNumber,
                    request.PageSize
                );

                return Result<JobPortal.CQRS.Common.Models.PagedResult<UserDto>>.Success(pagedResult);
            }
            catch (Exception ex)
            {
                return Result<JobPortal.CQRS.Common.Models.PagedResult<UserDto>>.Failure($"An error occurred while retrieving users: {ex.Message}");
            }
        }
    }
} 