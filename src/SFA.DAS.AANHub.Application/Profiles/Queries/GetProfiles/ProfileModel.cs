using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class ProfileModel
    {
        public static implicit operator ProfileModel(Profile source) => new ProfileModel
        {
            Id = source.Id,
            Category = source.Category,
            Description = source.Description,
            Ordering = source.Ordering
        };
        public long Id { get; set; }
        public string Description { get; set; } = null!;
        public string Category { get; set; } = null!;
        public int Ordering { get; set; }
    }
}