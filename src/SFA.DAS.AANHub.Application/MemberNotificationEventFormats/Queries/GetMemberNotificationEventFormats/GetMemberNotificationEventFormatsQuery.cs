using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;

namespace SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQuery
    : IRequest<GetMemberNotificationEventFormatsQueryResult>, IMemberId
{
    public Guid MemberId { get; set; }
}
