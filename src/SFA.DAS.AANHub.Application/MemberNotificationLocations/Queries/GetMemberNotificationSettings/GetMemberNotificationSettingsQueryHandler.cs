using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings;

public class GetMemberNotificationSettingsQueryHandler(IMembersReadRepository memberRepository)
    : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
{
    public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
    {
        var member = await memberRepository.GetMember(request.MemberId);

        if (member == null)
        {
            throw new InvalidOperationException($"Unable to find member {request.MemberId}");
        }

        return new GetMemberNotificationSettingsQueryResult
        {
            ReceiveNotifications = member.ReceiveNotifications,
            EventTypes = member.MemberNotificationEventFormats.Select(x =>  new GetMemberNotificationSettingsQueryResult.NotificationEventType
            {
                ReceiveNotifications = x.ReceiveNotifications,
                EventType = x.EventFormat
            }).ToList(),
            Locations = member.MemberNotificationLocations.Select(x => new GetMemberNotificationSettingsQueryResult.Location
            {
                Name = x.Name,
                Radius = x.Radius,
                Latitude = x.Latitude,
                Longitude = x.Longitude
            }).ToList()
        };
    }
}
