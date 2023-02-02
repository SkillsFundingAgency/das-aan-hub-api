using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;

namespace SFA.DAS.AANHub.Application.Mediatr.Responses
{
    [ExcludeFromCodeCoverage]
    public class ValidatedResponse
    {
    }

    [ExcludeFromCodeCoverage]
    public class ValidatedResponse<TModel> : ValidatedResponse where TModel : class
    {
        private readonly IList<ValidationFailure> _errorMessages = new List<ValidationFailure>();

        public ValidatedResponse(TModel model) => Result = model;
        public ValidatedResponse(IList<ValidationFailure> validationErrors) => _errorMessages = validationErrors;
        public TModel Result { get; } = null!;

        public IReadOnlyCollection<ValidationFailure> Errors => new ReadOnlyCollection<ValidationFailure>(_errorMessages);
        public bool IsValidResponse => !_errorMessages.Any();
    }
}