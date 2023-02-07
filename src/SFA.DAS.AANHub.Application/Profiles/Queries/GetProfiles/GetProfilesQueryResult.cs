using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQueryResult
    {
        public List<Profile> Profiles { get; set; } = new();

        public static implicit operator GetProfilesQueryResult(List<Profile> profiles) => new()
        {
            Profiles = profiles
        };
    }
}
