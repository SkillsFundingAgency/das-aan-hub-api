namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryResult
    {
        public List<ProfileModel> ProfileModels { get; set; } = new();

        public static implicit operator GetProfilesByUserTypeQueryResult(List<ProfileModel> profileModels) => new()
        {
            ProfileModels = profileModels
        };
    }
}
