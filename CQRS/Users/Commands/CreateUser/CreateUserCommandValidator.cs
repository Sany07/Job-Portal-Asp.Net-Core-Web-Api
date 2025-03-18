using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JobPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly JobPortalDbContext _dbContext;

        public CreateUserCommandValidator(JobPortalDbContext dbContext)
        {
            _dbContext = dbContext;

            // Username validation
            RuleFor(x => x.UserDto.Username)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, underscores, and hyphens")
                .MustAsync(IsUsernameUnique).WithMessage("Username is already taken");

            // Email validation
            RuleFor(x => x.UserDto.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .MustAsync(IsEmailUnique).WithMessage("Email is already registered");

            // Password validation
            RuleFor(x => x.UserDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                .WithMessage("Password must include at least one uppercase letter, one lowercase letter, one number, and one special character");

            // Confirm password validation
            RuleFor(x => x.UserDto.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.UserDto.Password).WithMessage("Password and confirmation password do not match");

            // First name validation
            RuleFor(x => x.UserDto.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            // Last name validation
            RuleFor(x => x.UserDto.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            // Phone number validation (optional)
            When(x => !string.IsNullOrEmpty(x.UserDto.PhoneNumber), () => {
                RuleFor(x => x.UserDto.PhoneNumber)
                    .Matches(@"^\+?[0-9\s-\(\)]+$").WithMessage("Invalid phone number format");
            });

            // Address validation (optional)
            When(x => !string.IsNullOrEmpty(x.UserDto.Address), () => {
                RuleFor(x => x.UserDto.Address)
                    .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
            });

            // Date of birth validation (optional)
            When(x => x.UserDto.DateOfBirth.HasValue, () => {
                RuleFor(x => x.UserDto.DateOfBirth)
                    .Must(dob => {
                        if (!dob.HasValue) return true;
                        var age = System.DateTime.Today.Year - dob.Value.Year;
                        if (dob.Value.Date > System.DateTime.Today.AddYears(-age)) age--;
                        return age >= 18;
                    }).WithMessage("User must be at least 18 years old");
            });
            
            // Gender validation
            RuleFor(x => x.UserDto.Gender)
                .IsInEnum().WithMessage("Invalid gender selection");
        }

        private async Task<bool> IsUsernameUnique(string username, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users
                .AnyAsync(u => u.Username == username, cancellationToken);
        }

        private async Task<bool> IsEmailUnique(string email, CancellationToken cancellationToken)
        {
            return !await _dbContext.Users
                .AnyAsync(u => u.Email == email, cancellationToken);
        }
    }
} 