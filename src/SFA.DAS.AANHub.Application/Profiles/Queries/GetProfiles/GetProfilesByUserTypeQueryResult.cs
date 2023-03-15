namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryResult
    {
        public List<ProfileModel> Profiles { get; set; } = new();

        public static implicit operator GetProfilesByUserTypeQueryResult(List<ProfileModel> profileModels) => new()
        {
            Profiles = profileModels
        };
    }
}
