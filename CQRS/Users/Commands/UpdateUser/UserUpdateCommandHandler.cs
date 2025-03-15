using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using JobPortal.CQRS.Common;
using JobPortal.Data;
using JobPortal.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.UpdateUser
{
    public class UserUpdateCommandHandler : IRequestHandler<UserUpdateCommand, Result>
    {
        private readonly JobPortalDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAccountValidationService _validationService;

        public UserUpdateCommandHandler(
            JobPortalDbContext context,
            IMapper mapper,
            IAccountValidationService validationService)
        {
            _context = context;
            _mapper = mapper;
            _validationService = validationService;
        }

        public async Task<Result> Handle(UserUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find the user
                var user = await _context.Users
                    .FindAsync(new object[] { request.Id }, cancellationToken);

                if (user == null)
                {
                    return Result.Failure("User not found");
                }

                // Apply changes
                _mapper.Map(request.UserDto, user);

                // Validate the updated user
                var validationResult = await _validationService.ValidateUserAsync(user);
                if (!validationResult.IsValid)
                {
                    return Result.Failure(string.Join(", ", validationResult.ErrorMessages));
                }

                // Update user in the database
                _context.Users.Update(user);
                await _context.SaveChangesAsync(cancellationToken);

                return Result.Success();
            }
            catch (DbUpdateException ex)
            {
                return Result.Failure("Error updating user. The username or email might already be in use.");
            }
            catch (Exception ex)
            {
                return Result.Failure($"An error occurred while updating the user: {ex.Message}");
            }
        }
    }
} 