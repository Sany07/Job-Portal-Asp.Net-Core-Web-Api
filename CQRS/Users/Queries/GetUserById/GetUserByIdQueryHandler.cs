using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly JobPortalDbContext _context;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(JobPortalDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

                if (user == null)
                {
                    return Result.Failure<UserDto>($"User with ID {request.Id} not found");
                }

                var userDto = _mapper.Map<UserDto>(user);
                return Result.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result.Failure<UserDto>($"An error occurred while retrieving the user: {ex.Message}");
            }
        }
    }
} 