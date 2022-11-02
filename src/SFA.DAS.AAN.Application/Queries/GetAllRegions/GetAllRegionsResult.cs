using SFA.DAS.AAN.Domain.Entities;

namespace SFA.DAS.AAN.Application.Queries.GetAllRegions
{
    public class GetAllRegionsResult
    {
        public ICollection<Region>? Regions { get; set; }
    }
}
