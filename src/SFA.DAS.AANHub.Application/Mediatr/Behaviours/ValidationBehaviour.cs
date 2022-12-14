using FluentValidation;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Mediatr.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);

            var failures = _validators
                .Select(x => x.ValidateAsync(context, cancellationToken).GetAwaiter().GetResult())
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next();
        }
    }
}
