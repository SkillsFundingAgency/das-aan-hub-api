using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQuery : IRequest<ValidatedResponse<GetProfilesByUserTypeQueryResult>>
    {
        public GetProfilesByUserTypeQuery(string userType) => UserType = userType;
        public string UserType { get; }
    }
}
