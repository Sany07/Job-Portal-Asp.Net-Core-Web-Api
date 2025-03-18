using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common.Models;
using JobPortal.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.UpdateUser
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<bool>>
    {
        private readonly JobPortalDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(JobPortalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<bool>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user by ID
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

                if (user == null)
                {
                    return Result<bool>.Failure("User not found");
                }

                // Update user properties using AutoMapper
                // This will apply non-null values from DTO to the entity
                _mapper.Map(request.UserDto, user);
                
                // Save changes
                await _dbContext.SaveChangesAsync(cancellationToken);
                
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"An error occurred while updating the user: {ex.Message}");
            }
        }
    }
} 