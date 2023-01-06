using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.Apprentices.Commands;
using SFA.DAS.AANHub.Application.UnitTests;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers
{
    public class ApprenticesControllerTests
    {
        [Test, AutoMoqData]
        public async Task CreateApprentice_InvokesRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Greedy] ApprenticesController sut,
            CreateApprenticeModel model, CreateApprenticeMemberCommand command, Guid userId, long apprenticeId)
        {
            var response = new CreateApprenticeMemberCommandResponse
            {
                MemberId = command.Id,
                Status = MembershipStatus.Live
            };

            mediatorMock.Setup(m => m.Send(It.Is<CreateApprenticeMemberCommand>(c => c.RequestedByMemberId == userId && c.ApprenticeId == apprenticeId), It.IsAny<CancellationToken>())).ReturnsAsync(response);
            var result = await sut.CreateApprentice(Guid.NewGuid(), model);

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Apprentice");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("CreateApprentice");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }
    }
}
