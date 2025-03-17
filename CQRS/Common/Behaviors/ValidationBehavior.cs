using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using JobPortal.CQRS.Common.Models;
using MediatR;

namespace JobPortal.CQRS.Common.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest>(request);
            var validationResults = await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

            var failures = validationResults
                .SelectMany(r => r.Errors)
                .Where(f => f != null)
                .ToList();

            if (failures.Count == 0)
            {
                return await next();
            }

            // If the response is of type Result or Result<T>, return a failure result
            if (typeof(TResponse).IsGenericType && 
                (typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>)))
            {
                var errorMessages = failures.Select(x => x.ErrorMessage).ToList();
                
                // Create a failure Result<T>
                var resultType = typeof(TResponse).GetGenericArguments()[0];
                var resultMethod = typeof(Result<>).MakeGenericType(resultType)
                    .GetMethod("Failure", new[] { typeof(IEnumerable<string>) });
                
                return resultMethod.Invoke(null, new object[] { errorMessages }) as TResponse;
            }

            // For other response types, throw a validation exception
            throw new ValidationException(failures);
        }
    }
} 