using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Api.Common
{
    public class ActionResponseControllerBase : ControllerBase
    {
        private readonly ILogger<ActionResponseControllerBase> _logger;

        public ActionResponseControllerBase(ILogger<ActionResponseControllerBase> logger) => _logger = logger;

        protected IActionResult GetResponse<T>(ValidatedResponse<T> response, BaseRequestDetails requestDetails) where T : class
        {
            if (response.Result == null && response.Errors.Count == 0)
            {
                _logger.LogTrace("No data found for {controllerName}: {actionName}. Id: {id}",
                    requestDetails.ControllerName,
                    requestDetails.ActionName,
                    requestDetails.GetId);

                return NotFound();
            }

            if (response.IsValidResponse)
            {
                _logger.LogTrace("Successful GET request for {controllerName}: {actionName}. Id: {id}",
                    requestDetails.ControllerName,
                    requestDetails.ActionName,
                    requestDetails.GetId);

                return new OkObjectResult(response.Result);
            }

            _logger.LogTrace("Bad GET request for {controllerName}: {actionName}. Id: {id}",
                requestDetails.ControllerName,
                requestDetails.ActionName,
                requestDetails.GetId);

            return new BadRequestObjectResult(FormatErrors(response));
        }

        protected IActionResult GetPostResponse<T>(ValidatedResponse<T> response, BaseRequestDetails requestDetails) where T : class
        {
            if (response.IsValidResponse)
            {
                _logger.LogTrace("Successful POST request for {controllerName}: {actionName}. Id: {id}",
                    requestDetails.ControllerName,
                    requestDetails.ActionName,
                    requestDetails.GetId);

                return new CreatedAtActionResult(requestDetails.ActionName,
                    requestDetails.ControllerName,
                    requestDetails.GetParameters,
                    response.Result);
            }

            _logger.LogTrace("Bad POST request for {controllerName}: {actionName}",
                requestDetails.ControllerName,
                requestDetails.ActionName);

            return new BadRequestObjectResult(FormatErrors(response));
        }

        private static List<ValidationError> FormatErrors<T>(ValidatedResponse<T> response) where T : class
        {
            return response.Errors.Select(err => new ValidationError
            {
                PropertyName = err.PropertyName,
                ErrorMessage = err.ErrorMessage
            }).ToList();
        }
    }
}