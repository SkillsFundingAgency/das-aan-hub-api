using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class MemberNotificationLocationsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetMemberNotificationLocations_InvokesQueryHandler(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] MemberNotificationLocationsController sut,
    Guid memberId,
    CancellationToken cancellationToken)
    {
        await sut.GetMemberNotificationLocations(memberId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetMemberNotificationLocationsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task GetMemberNotificationLocations_HandlerReturnsData_ReturnsOkResponse(
        bool IsPublicView,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberNotificationLocationsController sut,
        Guid memberId,
        GetMemberNotificationLocationsQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        ;
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberNotificationLocationsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetMemberNotificationLocations(memberId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<GetMemberNotificationLocationsQueryResult>();
        result.As<OkObjectResult>().Value.As<GetMemberNotificationLocationsQueryResult>()
            .MemberNotificationLocations.Should().HaveCount(queryResult.MemberNotificationLocations.Count());
    }
}
