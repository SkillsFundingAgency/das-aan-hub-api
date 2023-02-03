﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AANHub.Api.Common;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Application.Employers.Queries;
using System.ComponentModel.DataAnnotations;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace SFA.DAS.AANHub.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployersController : ControllerBase
    {
        private readonly ILogger<EmployersController> _logger;
        private readonly IMediator _mediator;
        public EmployersController(ILogger<EmployersController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        /// <summary>
        /// Creates an employer member
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateEmployer([FromHeader(Name = Constants.PostRequestHeaders.RequestedByUserHeader), Required] Guid? userId, CreateEmployerModel request)
        {
            _logger.LogInformation("AAN Hub API: Received command to add employer by accountId: {accountId} and UserId: {userId}:", request.AccountId, request.UserId);

            CreateEmployerMemberCommand command = request;
            command.RequestedByMemberId = userId;

            var response = await _mediator.Send(command);

            return response.IsValidResponse ? new CreatedAtActionResult(nameof(CreateEmployer), "Employers", new { id = response.Result.MemberId }, response.Result)
            : new BadRequestObjectResult(response.Errors);

        }

        /// <summary>
        /// Gets an employer member
        /// </summary>
        /// <param name="externalUserId"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/employers/{accountId}/users/{externalUserId}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(GetEmployerMemberResult), StatusCodes.Status200OK)]
        public async Task<ActionResult> GetEmployer(long accountId, long externalUserId)
        {
            _logger.LogInformation("AAN Hub API: Received command to get employer by AccountId: {accountId} and ExternalUserId: {externalUserId}", accountId, externalUserId);

            var response = await _mediator.Send(new GetEmployerMemberQuery(accountId, externalUserId));

            if (response.Result == null && (response.Errors.Count == 0 || response.Errors == null))
                return NotFound();

            return response.IsValidResponse ? new OkObjectResult(response.Result): new BadRequestObjectResult(response.Errors);
        }
    }
}
