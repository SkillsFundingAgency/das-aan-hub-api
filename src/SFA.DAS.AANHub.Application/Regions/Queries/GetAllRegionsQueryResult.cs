using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Regions.Queries
{
    public class GetAllRegionsQueryResult
    {
        public List<Region> Regions { get; set; } = new List<Region>();
    }
}
