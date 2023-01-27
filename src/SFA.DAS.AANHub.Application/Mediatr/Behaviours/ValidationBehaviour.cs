using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Mediatr.Behaviours
{
    [ExcludeFromCodeCoverage]
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TResponse : ValidatedResponse
        where TRequest : IRequest<TResponse>
    {
        private readonly IValidator<TRequest> _compositeValidator;
        private readonly ILogger<TRequest> _logger;

        public ValidationBehaviour(IValidator<TRequest> compositeValidator, ILogger<TRequest> logger)
        {
            _compositeValidator = compositeValidator;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Validating AANHub API request");

            var result = await _compositeValidator.ValidateAsync(request, cancellationToken);

            if (!result.IsValid)
            {
                var errors = result.Errors.Select(s => s.ErrorMessage).Aggregate(
                    (acc, current) => acc + string.Concat(' ', current)
                );

                _logger.LogTrace("{errors}", errors);

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    var resultType = responseType.GetGenericArguments()[0];
                    var invalidResponseType = typeof(ValidatedResponse<>).MakeGenericType(resultType);

                    if (Activator.CreateInstance(invalidResponseType,
                            result.Errors) as TResponse is { } invalidResponse)
                        return invalidResponse;
                }
            }

            _logger.LogTrace("Validation passed");

            var response = await next();

            return response;
        }
    }
}