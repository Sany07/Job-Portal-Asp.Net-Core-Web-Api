using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JobPortal.Data;
using Microsoft.EntityFrameworkCore;

namespace JobPortal.CQRS.Users.Queries.GetUserById
{
    public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
    {
        private readonly JobPortalDbContext _dbContext;

        public GetUserByIdQueryValidator(JobPortalDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("User ID must be greater than 0")
                .MustAsync(UserExists).WithMessage("User not found");
        }

        private async Task<bool> UserExists(int userId, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Id == userId, cancellationToken);
        }
    }
} 