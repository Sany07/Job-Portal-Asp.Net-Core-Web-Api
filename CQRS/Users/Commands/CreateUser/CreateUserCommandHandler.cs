using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common;
using JobPortal.Data;
using JobPortal.Models;
using JobPortal.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<int>>
    {
        private readonly JobPortalDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountValidationService _validationService;

        public CreateUserCommandHandler(
            JobPortalDbContext context,
            IMapper mapper,
            IAccountValidationService validationService)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Map DTO to domain entity
                var user = _mapper.Map<User>(request.UserDto);
                
                // Set creation date
                user.CreatedAt = DateTime.UtcNow;

                // Validate uniqueness
                var validationResult = await _validationService.ValidateUserAsync(user);
                if (!validationResult.IsValid)
                {
                    return Result.Failure<int>(string.Join(", ", validationResult.ErrorMessages));
                }

                // Add to database
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);
                
                return Result.Success(user.Id);
            }
            catch (DbUpdateException ex)
            {
                // Handle potential race condition
                return Result.Failure<int>("Error creating user. The username or email might already be in use.");
            }
            catch (Exception ex)
            {
                return Result.Failure<int>($"An error occurred while creating the user: {ex.Message}");
            }
        }
    }
} 