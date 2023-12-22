using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.LeavingReasons.Queries.GetLeavingReasons;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.LeavingReasons.Queries;

public class WhenRequestingGetLeavingReasons
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnAllRegions(
        GetLeavingReasonsQuery query,
        [Frozen] Mock<ILeavingReasonsReadRepository> readRepositoryMock,
        GetLeavingReasonsQueryHandler handler,
        List<LeavingReason> leavingReasons,
        CancellationToken cancellationToken)
    {
        readRepositoryMock.Setup(r => r.GetAllLeavingReasons(cancellationToken)).ReturnsAsync(leavingReasons);

        var result = await handler.Handle(query, cancellationToken);

        result?.LeavingReasons.Should().BeEquivalentTo(leavingReasons);
    }
}