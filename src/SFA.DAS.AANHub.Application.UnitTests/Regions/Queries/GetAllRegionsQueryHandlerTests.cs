using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Regions.Queries;

public class WhenRequestingGetAllRegions
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnAllRegions(
        GetRegionsQuery query,
        Mock<IRegionsReadRepository> regionsReadRepositoryMock,
        GetRegionsQueryHandler handler,
        List<Region> regions)
    {
        regionsReadRepositoryMock.Setup(r => r.GetAllRegions()).ReturnsAsync(regions);

        var result = await handler.Handle(query, CancellationToken.None);

        result?.Result.Regions?.Count.Should().Be(regions.Count);
    }
}