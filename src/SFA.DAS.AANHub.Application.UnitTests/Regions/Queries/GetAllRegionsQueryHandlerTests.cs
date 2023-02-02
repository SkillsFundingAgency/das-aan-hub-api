using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;
using SFA.DAS.AANHub.Data;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.Regions.Queries
{
    public class WhenRequestingGetAllRegions
    {
        [Test]
        [AutoMoqData]
        public async Task Handle_ReturnAllRegions(
            GetRegionsQuery query,
            [Frozen(Matching.ImplementedInterfaces)]
            AanDataContext context,
            GetRegionsQueryHandler handler,
            List<Region> response)
        {
            context.Regions.AddRange(response);
            await context.SaveChangesAsync();

            var result = await handler.Handle(query, CancellationToken.None);

            result?.Result.Regions?.Count.Should().Be(response.Count);
        }
    }
}