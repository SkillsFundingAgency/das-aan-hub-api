﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Application.UnitTests;
using SFA.DAS.AANHub.Domain.Enums;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class EmployersControllerTests
    {
        [Test, AutoMoqData]
        public async Task CreateEmployer_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] EmployersController sut,
            CreateEmployerMemberCommand model, long userId, long accountId, string organisation)
        {
            var response = new CreateEmployerMemberCommandResponse
            {
                MemberId = model.Id,
                Status = MembershipStatus.Live.ToString(),
            };

            mediatorMock.Setup(m => m.Send(It.Is<CreateEmployerMemberCommand>(c => c.UserId == userId && c.Organisation == organisation && c.AccountId == accountId), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await sut.CreateEmployer(new RequestHeaders() { RequestedByUserId = Guid.NewGuid() }, model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Employer");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateEmployer");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(201);
        }
    }
}
