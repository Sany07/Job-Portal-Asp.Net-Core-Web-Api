using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
    {
        private readonly JobPortalDbContext _context;
        private readonly IMapper _mapper;

        public GetUsersQueryHandler(JobPortalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Start with all users
                var query = _context.Users.AsNoTracking();

                // Apply search filter if provided
                if (!string.IsNullOrWhiteSpace(request.SearchTerm))
                {
                    var searchTerm = request.SearchTerm.ToLower();
                    query = query.Where(u => 
                        u.Username.ToLower().Contains(searchTerm) || 
                        u.Email.ToLower().Contains(searchTerm) || 
                        u.FirstName.ToLower().Contains(searchTerm) || 
                        u.LastName.ToLower().Contains(searchTerm));
                }

                // Get total count for pagination
                var totalCount = await query.CountAsync(cancellationToken);

                // Apply pagination
                var pagedUsers = await query
                    .OrderBy(u => u.Username)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                // Map to DTOs
                var userDtos = _mapper.Map<List<UserDto>>(pagedUsers);

                // Return paged result
                return PagedResult<UserDto>.Success(
                    userDtos, 
                    totalCount, 
                    request.PageNumber, 
                    request.PageSize);
            }
            catch (Exception ex)
            {
                return PagedResult<UserDto>.Failure($"An error occurred while retrieving users: {ex.Message}");
            }
        }
    }
}