using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers;

public class MemberNotificationEventFormatsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetMemberNotificationEventFormats_InvokesQueryHandler(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] MemberNotificationEventFormatsController sut,
    Guid memberId,
    CancellationToken cancellationToken)
    {
        await sut.GetMemberNotificationEventFormats(memberId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetMemberNotificationEventFormatsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task GetMemberNotificationEventFormats_HandlerReturnsData_ReturnsOkResponse(
        bool IsPublicView,
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MemberNotificationEventFormatsController sut,
        Guid memberId,
        GetMemberNotificationEventFormatsQueryResult queryResult,
        CancellationToken cancellationToken)
    {;
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberNotificationEventFormatsQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetMemberNotificationEventFormats(memberId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().BeOfType<GetMemberNotificationEventFormatsQueryResult>();
        result.As<OkObjectResult>().Value.As<GetMemberNotificationEventFormatsQueryResult>()
            .MemberNotificationEventFormats.Should().HaveCount(queryResult.MemberNotificationEventFormats.Count());
    }
}
