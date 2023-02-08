using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQuery : IRequest<ValidatedResponse<List<ProfileModel>>>
    {
        public GetProfilesQuery(string userType) => UserType = userType;
        public string UserType { get; }
    }
}
