using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JobPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.UpdateUser
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly JobPortalDbContext _dbContext;

        public UpdateUserCommandValidator(JobPortalDbContext dbContext)
        {
            _dbContext = dbContext;

            // User ID validation
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0")
                .MustAsync(UserExists).WithMessage("User not found");

            // First name validation (if provided)
            When(x => x.UserDto.FirstName != null, () => {
                RuleFor(x => x.UserDto.FirstName)
                    .NotEmpty().WithMessage("First name cannot be empty")
                    .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");
            });

            // Last name validation (if provided)
            When(x => x.UserDto.LastName != null, () => {
                RuleFor(x => x.UserDto.LastName)
                    .NotEmpty().WithMessage("Last name cannot be empty")
                    .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");
            });

            // Phone number validation (if provided)
            When(x => x.UserDto.PhoneNumber != null, () => {
                RuleFor(x => x.UserDto.PhoneNumber)
                    .Matches(@"^\+?[0-9\s-\(\)]+$").WithMessage("Invalid phone number format");
            });

            // Address validation (if provided)
            When(x => x.UserDto.Address != null, () => {
                RuleFor(x => x.UserDto.Address)
                    .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
            });

            // Date of birth validation (if provided)
            When(x => x.UserDto.DateOfBirth.HasValue, () => {
                RuleFor(x => x.UserDto.DateOfBirth)
                    .Must(dob => {
                        if (!dob.HasValue) return true;
                        var age = System.DateTime.Today.Year - dob.Value.Year;
                        if (dob.Value.Date > System.DateTime.Today.AddYears(-age)) age--;
                        return age >= 18;
                    }).WithMessage("User must be at least 18 years old");
            });
            
            // Gender validation (if provided)
            When(x => x.UserDto.Gender.HasValue, () => {
                RuleFor(x => x.UserDto.Gender)
                    .IsInEnum().WithMessage("Invalid gender selection");
            });
        }

        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Id == userId, cancellationToken);
        }
    }
} 