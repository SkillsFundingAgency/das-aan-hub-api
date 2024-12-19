using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQuery : IRequest<GetMemberNotificationSettingsQueryResult>, IMemberId
{
    public Guid MemberId { get; set; }
}
