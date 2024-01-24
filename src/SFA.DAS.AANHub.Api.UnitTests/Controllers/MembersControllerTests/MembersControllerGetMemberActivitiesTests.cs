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
    public async Task GetMemberActivities_InvokesQueryHandler(GetMemberActivitiesResult getMemberActivitiesResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        await sut.GetMemberActivities(memberId);

        // Assert
        mediatorMock.Verify(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()));
    }

    [Test, AutoData]
    public async Task GetMemberActivities_HandlerReturnsData_ReturnsOkResponse(
        GetMemberActivitiesResult getMemberActivitiesResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);

        // Assert
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getMemberActivitiesResult);
    }

    [Test, AutoData]
    public async Task GetMemberActivities_HandlerReturnsData_ShouldReturnExpectedValue(
    GetMemberActivitiesResult getMemberActivitiesResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesResult)objectResult!.Value!;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(memberActivitiesResult, Is.Not.Null);
            Assert.That(memberActivitiesResult.LastSignedUpDate, Is.EqualTo(getMemberActivitiesResult.LastSignedUpDate));
            Assert.That(memberActivitiesResult.EventsPlanned, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsPlanned.EventsDateRange, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.EventsDateRange));
            Assert.That(memberActivitiesResult.EventsPlanned.EventsDateRange.FromDate, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.EventsDateRange.FromDate));
            Assert.That(memberActivitiesResult.EventsPlanned.EventsDateRange.ToDate, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.EventsDateRange.ToDate));
            Assert.That(memberActivitiesResult.EventsPlanned.Events.Count, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.Events.Count));
            Assert.That(memberActivitiesResult.EventsAttended, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsAttended.EventsDateRange, Is.EqualTo(getMemberActivitiesResult.EventsAttended.EventsDateRange));
            Assert.That(memberActivitiesResult.EventsAttended.EventsDateRange.FromDate, Is.EqualTo(getMemberActivitiesResult.EventsAttended.EventsDateRange.FromDate));
            Assert.That(memberActivitiesResult.EventsAttended.EventsDateRange.ToDate, Is.EqualTo(getMemberActivitiesResult.EventsAttended.EventsDateRange.ToDate));
            Assert.That(memberActivitiesResult.EventsAttended.Events.Count, Is.EqualTo(getMemberActivitiesResult.EventsAttended.Events.Count));
        });
    }

    [Test, AutoData]
    public async Task GetMemberActivities_ReturnEmptyActivityListForMember(
        GetMemberActivitiesResult getMemberActivitiesResult)
    {
        // Arrange
        getMemberActivitiesResult.EventsAttended.Events = new();
        getMemberActivitiesResult.EventsPlanned.Events = new();
        var response = new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesResult)objectResult!.Value!;

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
        GetMemberActivitiesResult getMemberActivitiesResult)
    {
        // Arrange
        var response = new ValidatedResponse<GetMemberActivitiesResult>(getMemberActivitiesResult);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);

        // Act
        var result = await sut.GetMemberActivities(memberId);
        var objectResult = result as OkObjectResult;
        var memberActivitiesResult = (GetMemberActivitiesResult)objectResult!.Value!;

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(memberActivitiesResult, Is.Not.Null);
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].CalendarEventId, Is.EqualTo(getMemberActivitiesResult.EventsAttended.Events[0].CalendarEventId));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].EventDate, Is.EqualTo(getMemberActivitiesResult.EventsAttended.Events[0].EventDate));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].EventTitle, Is.EqualTo(getMemberActivitiesResult.EventsAttended.Events[0].EventTitle));
            Assert.That(memberActivitiesResult.EventsAttended.Events[0].Urn, Is.EqualTo(getMemberActivitiesResult.EventsAttended.Events[0].Urn));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].CalendarEventId, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.Events[0].CalendarEventId));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].EventDate, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.Events[0].EventDate));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].EventTitle, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.Events[0].EventTitle));
            Assert.That(memberActivitiesResult.EventsPlanned.Events[0].Urn, Is.EqualTo(getMemberActivitiesResult.EventsPlanned.Events[0].Urn));
        });
    }

    [Test, AutoData]
    public async Task GetMember_InvalidRequest_ReturnsBadRequestResponse(
        List<ValidationFailure> errors)
    {
        // Arrange
        var errorResponse = new ValidatedResponse<GetMemberActivitiesResult>(errors);
        mediatorMock.Setup(m => m.Send(It.Is<GetMemberActivitiesQuery>(q => q.MemberId == memberId), It.IsAny<CancellationToken>())).ReturnsAsync(errorResponse);

        // Act
        var result = await sut.GetMemberActivities(memberId);

        // Assert
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
