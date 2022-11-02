using AutoFixture.Xunit2;
using FluentAssertions;
using SFA.DAS.AAN.Application.Queries.GetAllRegions;
using SFA.DAS.AAN.Data;
using SFA.DAS.AAN.Domain.Entities;

namespace SFA.DAS.AAN.Application.UnitTests.Queries
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
