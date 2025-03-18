using System.Linq;
using FluentValidation;

namespace JobPortal.CQRS.Users.Queries.GetUsers
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThan(0).WithMessage("Page number must be greater than 0");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50");

            When(x => !string.IsNullOrEmpty(x.SortColumn), () => {
                RuleFor(x => x.SortColumn)
                    .Must(BeValidSortColumn).WithMessage("Invalid sort column");
            });

            When(x => !string.IsNullOrEmpty(x.SortDirection), () => {
                RuleFor(x => x.SortDirection)
                    .Must(direction => direction.ToLower() == "asc" || direction.ToLower() == "desc")
                    .WithMessage("Sort direction must be 'asc' or 'desc'");
            });
        }

        private bool BeValidSortColumn(string columnName)
        {
            var validColumns = new[] { 
                "username", "email", "firstname", "lastname", "createdat" 
            };
            
            return string.IsNullOrEmpty(columnName) || 
                   validColumns.Contains(columnName.ToLower());
        }
    }
} 