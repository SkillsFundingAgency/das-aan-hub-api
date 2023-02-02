using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Regions.Queries.GetRegions
{
    public class GetRegionsQueryResult
    {
        public List<Region> Regions { get; set; } = new();

        public static implicit operator GetRegionsQueryResult(List<Region> regions) => new()
        {
            Regions = regions
        };
    }
}