using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Controllers;
using SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Api.UnitTests.Controllers.AttendancesControllerTests;

public class AttendancesControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task Get_InvokesHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AttendancesController sut,
        Guid memberId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        await sut.Get(memberId, fromDate, toDate, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetMemberAttendancesQuery>(q => q.RequestedByMemberId == memberId && q.FromDate == fromDate && q.ToDate == toDate), cancellationToken));
    }

    [Test]
    [MoqAutoData]
    public async Task Get_RequestIsInvalid_ReturnsBadRequestResponse(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] AttendancesController sut,
    Guid memberId,
    DateTime fromDate,
    DateTime toDate,
    List<ValidationFailure> validationFailures,
    CancellationToken cancellationToken)
    {

        mediatorMock.Setup(m => m.Send(It.IsAny<GetMemberAttendancesQuery>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<GetMemberAttendancesQueryResult>(validationFailures));

        var response = await sut.Get(memberId, fromDate, toDate, cancellationToken);

        response.As<BadRequestObjectResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task Get_RequestIsValid_ReturnOkResponse(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] AttendancesController sut,
    Guid memberId,
    DateTime fromDate,
    DateTime toDate,
    GetMemberAttendancesQueryResult expectedResult,
    CancellationToken cancellationToken)
    {

        mediatorMock.Setup(m => m.Send(It.IsAny<GetMemberAttendancesQuery>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<GetMemberAttendancesQueryResult>(expectedResult));

        var response = await sut.Get(memberId, fromDate, toDate, cancellationToken);

        response.As<OkObjectResult>().Value.Should().Be(expectedResult);
    }
}
