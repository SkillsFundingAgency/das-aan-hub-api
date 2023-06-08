using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Attendances.Queries.GetMemberAttendances;

public class GetMemberAttendancesQueryHandlerTests
{
    private Mock<IAttendancesReadRepository> _repositoryMock = null!;
    private ValidatedResponse<GetMemberAttendancesQueryResult> _response = null!;
    private GetMemberAttendancesQuery _request = null!;

    [SetUp]
    public async Task SetUp()
    {
        Fixture fixture = new();
        _request = fixture.Create<GetMemberAttendancesQuery>();
        var attendances = AttendanceTestDataHelper.GetAttendances();

        _repositoryMock = new();
        _repositoryMock
            .Setup(r => r.GetAttendances(_request.RequestedByMemberId, _request.FromDate.GetValueOrDefault(), _request.ToDate.GetValueOrDefault().AddDays(1), It.IsAny<CancellationToken>()))
            .ReturnsAsync(attendances);

        GetMemberAttendancesQueryHandler sut = new(_repositoryMock.Object);
        _response = await sut.Handle(_request, CancellationToken.None);
    }

    [Test]
    public void ThenInvokesRepositoryWithAdvancedToDate()
    {
        _repositoryMock
            .Verify(r => r.GetAttendances(_request.RequestedByMemberId, _request.FromDate.GetValueOrDefault(), _request.ToDate.GetValueOrDefault().AddDays(1), It.IsAny<CancellationToken>()));
    }

    [Test]
    public void ThenReturnsValidatedResponseWithResults()
    {
        _response.Result.Attendances.Count.Should().BeGreaterThan(1);
    }
}
