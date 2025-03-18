using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common.Models;
using JobPortal.Data;
using JobPortal.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly JobPortalDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(JobPortalDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map DTO to domain entity
                var user = _mapper.Map<User>(request.UserDto);

                // Hash the password before storing
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.UserDto.Password);
                
                // Set creation date
                user.CreatedAt = DateTime.UtcNow;
                
                // Add the user to the database
                await _dbContext.Users.AddAsync(user, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                
                // Return the new user's ID
                return Result<int>.Success(user.Id);
            }
            catch (Exception ex)
            {
                // Log the exception (in a real-world scenario)
                return Result<int>.Failure($"An error occurred while creating the user: {ex.Message}");
            }
        }
    }
} 