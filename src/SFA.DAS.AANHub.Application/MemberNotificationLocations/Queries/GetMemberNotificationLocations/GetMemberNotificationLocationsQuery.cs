
using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

public class GetMemberNotificationLocationsQuery
    : IRequest<GetMemberNotificationLocationsQueryResult>, IMemberId
{
    public Guid MemberId { get; set; }
}
