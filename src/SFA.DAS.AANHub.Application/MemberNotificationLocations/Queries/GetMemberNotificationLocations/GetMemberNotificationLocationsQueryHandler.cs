using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

public class GetMemberNotificationLocationsQueryHandler : IRequestHandler<GetMemberNotificationLocationsQuery, GetMemberNotificationLocationsQueryResult>
{
    private readonly IMemberNotificationLocationReadRepository _memberNotificationLocationRepository;

    public GetMemberNotificationLocationsQueryHandler(IMemberNotificationLocationReadRepository memberNotificationLocationRepository)
    {
        _memberNotificationLocationRepository = memberNotificationLocationRepository;
    }

    public async Task<GetMemberNotificationLocationsQueryResult> Handle(GetMemberNotificationLocationsQuery request, CancellationToken cancellationToken)
    {
        var memberNotificationLocations = (await _memberNotificationLocationRepository
            .GetMemberNotificationLocationsByMember(request.MemberId, cancellationToken))
            .Select(m => (MemberNotificationLocationModel)m);

       return new GetMemberNotificationLocationsQueryResult
        {
            MemberNotificationLocations = memberNotificationLocations
        };
    }
}
