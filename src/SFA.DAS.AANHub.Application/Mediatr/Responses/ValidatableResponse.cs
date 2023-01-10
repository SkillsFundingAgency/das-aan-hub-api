using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.AANHub.Application.Mediatr.Responses
{
    [ExcludeFromCodeCoverage]
    public class ValidatableResponse
    {
        private readonly IList<string> _errorMessages;

        public ValidatableResponse(IList<string>? errors = null) => _errorMessages = errors ?? new List<string>();

        public bool IsValidResponse => !_errorMessages.Any();

        public IReadOnlyCollection<string> Errors => new ReadOnlyCollection<string>(_errorMessages);
    }
    [ExcludeFromCodeCoverage]
    public class ValidatableResponse<TModel> : ValidatableResponse
        where TModel : class
    {
        public ValidatableResponse() { }
        public ValidatableResponse(TModel model, IList<string>? validationErrors = null)
            : base(validationErrors) => Result = model;

        public TModel Result { get; } = null!;
    }
}
