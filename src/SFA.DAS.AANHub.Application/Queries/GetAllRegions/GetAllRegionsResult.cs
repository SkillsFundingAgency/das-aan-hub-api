using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Queries.GetAllRegions
{
    public class GetAllRegionsResult
    {
        public ICollection<Region>? Regions { get; set; }
    }
}
