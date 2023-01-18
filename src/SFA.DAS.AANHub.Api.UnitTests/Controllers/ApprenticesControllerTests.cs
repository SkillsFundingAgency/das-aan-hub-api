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
using SFA.DAS.AANHub.Application.Apprentices.Queries;
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

            result.As<CreatedAtActionResult>().ControllerName.Should().Be("Apprentices");
            result.As<CreatedAtActionResult>().ActionName.Should().Be("GetApprentice");
            result.As<CreatedAtActionResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
        }

        [Test, AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        long apprenticeid,
        GetApprenticeMemberResult handlerResult)
        {
            mediatorMock.Setup(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(handlerResult);

            var response = await sut.GetApprentice(apprenticeid);
            response.Should().NotBeNull();

            var result = response.Result as OkObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, (result!).StatusCode);

            var queryResult = result.Value as GetApprenticeMemberResult;
            queryResult.Should().BeEquivalentTo(handlerResult);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>()));
        }

        [Test, AutoMoqData]
        public async Task GetApprentice_InvokesQueryHandler_NoResultGivesNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] ApprenticesController sut,
        GetApprenticeMemberQuery query)
        {
            long apprenticeId = 0;
            mediatorMock.Setup(m => m.Send(It.Is<GetApprenticeMemberQuery>(q => q.ApprenticeId == apprenticeId ), It.IsAny<CancellationToken>())).ReturnsAsync((GetApprenticeMemberResult?) null!);

            var response = await sut.GetApprentice(apprenticeId);

            var result = response.Result as NotFoundResult;
            result.Should().NotBeNull();
            Assert.AreEqual(StatusCodes.Status404NotFound, (result!).StatusCode);

            mediatorMock.Verify(m => m.Send(It.IsAny<GetApprenticeMemberQuery>(), It.IsAny<CancellationToken>()));
        }
    }
}
