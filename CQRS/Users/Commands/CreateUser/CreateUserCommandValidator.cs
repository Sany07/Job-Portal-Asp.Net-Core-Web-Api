using FluentValidation;
using JobPortal.CQRS.Users.Commands.CreateUser;

namespace JobPortal.CQRS.Users.Commands.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserDto.Username)
                .NotEmpty().WithMessage("Username is required")
                .Length(3, 50).WithMessage("Username must be between 3 and 50 characters")
                .Matches(@"^[a-zA-Z0-9_-]+$").WithMessage("Username can only contain letters, numbers, underscores, and hyphens");

            RuleFor(x => x.UserDto.Email)
                .NotEmpty().WithMessage("Email address is required")
                .EmailAddress().WithMessage("Invalid email address format")
                .MaximumLength(100).WithMessage("Email address cannot exceed 100 characters");

            RuleFor(x => x.UserDto.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
                    .WithMessage("Password must include at least one uppercase letter, one lowercase letter, one number, and one special character");

            RuleFor(x => x.UserDto.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm password is required")
                .Equal(x => x.UserDto.Password).WithMessage("Password and confirmation password do not match");

            RuleFor(x => x.UserDto.FirstName)
                .NotEmpty().WithMessage("First name is required")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

            RuleFor(x => x.UserDto.LastName)
                .NotEmpty().WithMessage("Last name is required")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

            When(x => !string.IsNullOrEmpty(x.UserDto.PhoneNumber), () => {
                RuleFor(x => x.UserDto.PhoneNumber)
                    .Matches(@"^\+?[0-9\s-()]+$").WithMessage("Invalid phone number format");
            });

            RuleFor(x => x.UserDto.Address)
                .MaximumLength(200).WithMessage("Address cannot exceed 200 characters");
        }
    }
} 