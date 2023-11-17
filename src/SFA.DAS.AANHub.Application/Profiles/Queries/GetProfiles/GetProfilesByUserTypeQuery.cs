using MediatR;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQuery : IRequest<GetProfilesByUserTypeQueryResult>
    {
        public GetProfilesByUserTypeQuery(UserType userType) => UserType = userType;
        public UserType UserType { get; }
    }
}
