using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Api.Common;

[ExcludeFromCodeCoverage]
public abstract class ActionResponseControllerBase : ControllerBase
{
    public const string GetMethodName = "Get";

    public abstract string ControllerName { get; }

    private static readonly IReadOnlyCollection<string> notFoundErrorMessages = GetNotFoundErrorMessages();

    protected IActionResult GetResponse<T>(ValidatedResponse<T> response) where T : class
    {
        if (response.Result == null && response.IsValidResponse) return NotFound();

        if (response.IsValidResponse) return new OkObjectResult(response.Result);

        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }

    protected IActionResult GetPostResponse<T>(ValidatedResponse<T> response, object? routeParameters) where T : class
    {
        if (response.IsValidResponse)
        {
            return new CreatedAtActionResult(GetMethodName, ControllerName, routeParameters, response.Result);
        }
        if (ErrorsContainNotFoundMessages(response.Errors))
        {
            return new NotFoundObjectResult(FormatErrors(response.Errors.Where(e => notFoundErrorMessages.Contains(e.ErrorMessage))));
        }
        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }

    protected IActionResult GetPatchResponse(ValidatedResponse<PatchCommandResult> response)
    {
        if (response.Result is { IsSuccess: false }) return NotFound();

        if (response.IsValidResponse) return NoContent();

        return new BadRequestObjectResult(FormatErrors(response.Errors));
    }

    private static List<ValidationError> FormatErrors(IEnumerable<ValidationFailure> errors)
    {
        return errors.Select(err => new ValidationError
        {
            PropertyName = err.PropertyName,
            ErrorMessage = err.ErrorMessage
        }).ToList();
    }

    private static IReadOnlyCollection<string> Get404WorthyErrorMessages()
    {
        return new[] 
        { 
            CreateAttendanceCommandValidator.EventNotFoundMessage,
        };
    }

    private static bool ErrorsContainNotFoundMessages(IReadOnlyCollection<ValidationFailure> errors) 
    {
        return errors.Select(e => e.ErrorMessage)
                     .Intersect(notFoundErrorMessages)
                     .Any();
    }
}