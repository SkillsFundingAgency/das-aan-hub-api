using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Mediatr.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.MembersControllerTests;
public class MembersControllerGetMemberActivitiesTests
{
    private readonly Guid memberId = Guid.NewGuid();
    private Mock<IMediator> mediatorMock = null!;
    private Mock<ILogger<MembersController>> validatorMock = null!;
    private MembersController sut = null!;

    [SetUp]
    public void SetUp()
    {
        mediatorMock = new();
        validatorMock = new();
        sut = new MembersController(validatorMock.Object, mediatorMock.Object);

    }

    [Test, AutoData]
    public async Task GetMemberActivities_InvokesQueryHandler(GetMemberActivitiesQueryResult getMemberActivitiesQueryResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetMemberActivities(memberId);

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [Test, AutoData]
    public async Task GetMemberActivities_HandlerReturnsData_ReturnsOkResponse(
        GetMemberActivitiesQueryResult getMemberActivitiesQueryResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);

        // Assert
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberActivitiesQueryResult);
    }

    [Test, AutoData]
    public async Task GetMemberActivities_HandlerReturnsData_ShouldReturnExpectedValue(
    GetMemberActivitiesQueryResult getMemberActivitiesQueryResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesQueryResult)objectResult!.Value!;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(memberActivitiesResult, Is.Not.Null);
            Assert.That(memberActivitiesResult.LastSignedUpDate, Is.EqualTo(getMemberActivitiesQueryResult.LastSignedUpDate));
            Assert.That(memberActivitiesResult.EventsPlanned.Events.Count, Is.EqualTo(getMemberActivitiesQueryResult.EventsPlanned.Events.Count));
            Assert.That(memberActivitiesResult.EventsAttended.Events.Count, Is.EqualTo(getMemberActivitiesQueryResult.EventsAttended.Events.Count));
        });
    }

    [Test, AutoData]
    public async Task GetMemberActivities_ReturnEmptyActivityListForMember(
        GetMemberActivitiesQueryResult getMemberActivitiesQueryResult)
    {
        // Arrange
        getMemberActivitiesQueryResult.EventsAttended.Events = new();
        getMemberActivitiesQueryResult.EventsPlanned.Events = new();
        var response = new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesQueryResult)objectResult!.Value!;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(memberActivitiesResult, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsPlanned.Events, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsPlanned.Events.Count, Is.EqualTo(0));
            Assert.That(memberActivitiesResult.EventsAttended.Events, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsAttended.Events.Count, Is.EqualTo(0));
        });
    }

    [Test, AutoData]
    public async Task GetMemberActivities_HandlerReturnsData_ShouldReturnExpectedValueForEvents(
        GetMemberActivitiesQueryResult getMemberActivitiesQueryResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesQueryResult>(getMemberActivitiesQueryResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesQueryResult)objectResult!.Value!;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(memberActivitiesResult, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].CalendarEventId, Is.EqualTo(getMemberActivitiesQueryResult.EventsAttended.Events[0].CalendarEventId));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].EventDate, Is.EqualTo(getMemberActivitiesQueryResult.EventsAttended.Events[0].EventDate));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].EventTitle, Is.EqualTo(getMemberActivitiesQueryResult.EventsAttended.Events[0].EventTitle));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].Urn, Is.EqualTo(getMemberActivitiesQueryResult.EventsAttended.Events[0].Urn));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].CalendarEventId, Is.EqualTo(getMemberActivitiesQueryResult.EventsPlanned.Events[0].CalendarEventId));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].EventDate, Is.EqualTo(getMemberActivitiesQueryResult.EventsPlanned.Events[0].EventDate));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].EventTitle, Is.EqualTo(getMemberActivitiesQueryResult.EventsPlanned.Events[0].EventTitle));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].Urn, Is.EqualTo(getMemberActivitiesQueryResult.EventsPlanned.Events[0].Urn));
        });
    }

    [Test, AutoData]
    public async Task GetMember_InvalidRequest_ReturnsBadRequestResponse(
        List<ValidationFailure> errors)
    {
        // Arrange
        var errorResponse = new ValidatedResponse<GetMemberActivitiesQueryResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        // Act
        var result = await sut.GetMemberActivities(memberId);

        // Assert
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
