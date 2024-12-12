using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;

namespace SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQuery
    : IRequest<ValidatedResponse<GetMemberNotificationEventFormatsQueryResult>>, IMemberId
{
    public Guid MemberId { get; set; }
}
