using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Api.Common
{
    public class ActionResponseControllerBase : ControllerBase
    {
        private readonly string _controllerName;
        private readonly ILogger<ActionResponseControllerBase> _logger;

        public ActionResponseControllerBase(ILogger<ActionResponseControllerBase> logger, string controllerName)
        {
            _logger = logger;
            _controllerName = controllerName;
        }

        protected IActionResult GetResponse<T>(ValidatedResponse<T> response) where T : class
        {
            if (response.Result == null && response.IsValidResponse) return NotFound();

            if (response.IsValidResponse) return new OkObjectResult(response.Result);

            return new BadRequestObjectResult(FormatErrors(response.Errors));
        }

        protected IActionResult GetPostResponse<T>(ValidatedResponse<T> response, ReferrerRouteDetails requestDetails) where T : class
        {
            if (response.IsValidResponse)
            {
                _logger.LogTrace("Successful POST request for {controllerName}: {actionName}.",
                    _controllerName,
                    requestDetails.ActionName);


                return new CreatedAtActionResult(requestDetails.ActionName,
                    _controllerName,
                    requestDetails.RouteParameters,
                    response.Result);
            }

            _logger.LogTrace("Bad POST request for {controllerName}: {actionName}",
                _controllerName,
                requestDetails.ActionName);

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