using AutoFixture.Xunit2;
using FluentAssertions;
using SFA.DAS.AANHub.Application.Queries.GetAllRegions;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Queries
{
    public class WhenRequestingGetAllRegions
    {
        [Theory, AutoMoqData]
        public async Task ThenAllRegionsAreReturned(
            GetAllRegionsQuery query,
            [Frozen(Matching.ImplementedInterfaces)] AanDataContext context,
            GetAllRegionsQueryHandler handler,
            List<Region> response)
        {

            context.Regions.AddRange(response);
            context.SaveChanges();

            var result = await handler.Handle(query, CancellationToken.None);

            result?.Regions?.Count.Should().Be(response.Count);
        }
    }
}
