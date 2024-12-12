using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationEventFormats.Queries.GetMemberNotificationEventFormats;

public class GetMemberNotificationEventFormatsQueryHandler : IRequestHandler<GetMemberNotificationEventFormatsQuery, ValidatedResponse<GetMemberNotificationEventFormatsQueryResult>>
{
    private readonly IMemberNotificationEventFormatsReadRepository _memberNotificationEventFormatsRepository;

    public GetMemberNotificationEventFormatsQueryHandler(IMemberNotificationEventFormatsReadRepository memberNotificationEventFormatsRepository)
    {
        _memberNotificationEventFormatsRepository = memberNotificationEventFormatsRepository;
    }

    public async Task<ValidatedResponse<GetMemberNotificationEventFormatsQueryResult>> Handle(GetMemberNotificationEventFormatsQuery request, CancellationToken cancellationToken)
    {
        var memberNotificationEventFormats = (await _memberNotificationEventFormatsRepository
            .GetMemberNotificationEventFormatsByMember(request.MemberId, cancellationToken))
            .Select(f => (MemberNotificationEventFormatModel)f);

        GetMemberNotificationEventFormatsQueryResult result = new GetMemberNotificationEventFormatsQueryResult 
        {
            MemberNotificationEventFormats = memberNotificationEventFormats
        };

        return new ValidatedResponse<GetMemberNotificationEventFormatsQueryResult>(result);
    }
}
