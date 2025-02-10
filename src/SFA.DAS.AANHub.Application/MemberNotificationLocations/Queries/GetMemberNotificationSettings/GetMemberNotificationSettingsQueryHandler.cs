using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.MemberNotificationLocations.Queries.GetMemberNotificationSettings
{
    public class GetMemberNotificationSettingsQueryHandler : IRequestHandler<GetMemberNotificationSettingsQuery, GetMemberNotificationSettingsQueryResult>
    {
        private readonly IMembersReadRepository _memberRepository;

        public GetMemberNotificationSettingsQueryHandler(IMembersReadRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<GetMemberNotificationSettingsQueryResult> Handle(GetMemberNotificationSettingsQuery request, CancellationToken cancellationToken)
        {
            var member = await _memberRepository.GetMember(request.MemberId);

            if (member == null)
            {
                throw new InvalidOperationException($"Unable to find member {request.MemberId}");
            }

            var eventTypes = member.MemberNotificationEventFormats.Select(x => new GetMemberNotificationSettingsQueryResult.NotificationEventType
            {
                ReceiveNotifications = x.ReceiveNotifications,
                EventType = x.EventFormat
            }).ToList();

            // Check if "All" is present and unpack it
            if (eventTypes.Any(e => e.EventType == "All"))
            {
                eventTypes.Clear();
                eventTypes.AddRange(new List<GetMemberNotificationSettingsQueryResult.NotificationEventType>
                {
                    new() { EventType = "InPerson", ReceiveNotifications = true },
                    new() { EventType = "Online", ReceiveNotifications = true },
                    new() { EventType = "Hybrid", ReceiveNotifications = true }
                });
            }

            return new GetMemberNotificationSettingsQueryResult
            {
                ReceiveNotifications = member.ReceiveNotifications,
                EventTypes = eventTypes,
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
}
