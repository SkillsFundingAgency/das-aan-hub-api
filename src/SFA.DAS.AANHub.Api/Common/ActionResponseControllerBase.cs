using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Common.Commands;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Api.Common
{
    public class ActionResponseControllerBase : ControllerBase
    {
        protected IActionResult GetResponse<T>(ValidatedResponse<T> response) where T : class
        {
            if (response.Result == null && response.IsValidResponse) return NotFound();

            if (response.IsValidResponse) return new OkObjectResult(response.Result);

            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPostResponse<T>(ValidatedResponse<T> response, ReferrerRouteDetails requestDetails) where T : class
        {
            if (response.IsValidResponse)
                return new CreatedAtActionResult(requestDetails.ActionName,
                    requestDetails.ControllerName,
                    requestDetails.RouteParameters,
                    response.Result);

            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPatchResponse<T>(ValidatedResponse<T> response) where T : PatchMemberCommandResponseBase
        {
            if (response.Result != null && !response.Result.IsSuccess) return NotFound();

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
    }
}