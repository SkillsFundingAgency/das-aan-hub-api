
using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationLocations;

public class GetMemberNotificationLocationsQuery
    : IRequest<ValidatedResponse<GetMemberNotificationLocationsQueryResult>>, IMemberId
{
    public Guid MemberId { get; set; }
}
