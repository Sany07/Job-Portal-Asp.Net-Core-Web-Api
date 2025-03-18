using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common.Models;
using JobPortal.CQRS.Users.DTOs;
using JobPortal.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserDto>>
    {
        private readonly JobPortalDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetUserByIdQueryHandler(JobPortalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<UserDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _dbContext.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                if (user == null)
                {
                    return Result<UserDto>.Failure("User not found");
                }

                // Map the entity to DTO
                var userDto = _mapper.Map<UserDto>(user);
                
                return Result<UserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure($"An error occurred while retrieving the user: {ex.Message}");
            }
        }
    }
} 