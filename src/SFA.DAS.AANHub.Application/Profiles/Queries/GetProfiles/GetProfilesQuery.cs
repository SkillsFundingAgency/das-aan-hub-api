using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesQuery : IRequest<ValidatedResponse<GetProfilesQueryResult>>
    {
    }
}
