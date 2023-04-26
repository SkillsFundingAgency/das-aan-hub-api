using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Regions.Queries.GetRegions;

public class GetRegionsQueryResult
{
    public IEnumerable<RegionModel> Regions { get; set; } = null!;

    public static implicit operator GetRegionsQueryResult(List<Region> regions) => new()
    {
        Regions = regions.Select(r => new RegionModel(r.Id, r.Area, r.Ordering))
    };
}
public record RegionModel(int Id, string Area, int Ordering);