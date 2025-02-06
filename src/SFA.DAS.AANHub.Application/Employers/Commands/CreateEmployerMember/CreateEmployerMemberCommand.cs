using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember
{
    public class CreateEmployerMemberCommand : CreateMemberCommandBase, IRequest<ValidatedResponse<CreateMemberCommandResponse>>
    {
        public long AccountId { get; init; }
        public Guid UserRef { get; init; }

        public static implicit operator Member(CreateEmployerMemberCommand command) => new()
        {
            Id = command.MemberId,
            UserType = UserType.Employer,
            Status = MembershipStatus.Live,
            Email = command.Email!,
            FirstName = command.FirstName!,
            LastName = command.LastName!,
            JoinedDate = command.JoinedDate!.Value,
            RegionId = command.RegionId,
            OrganisationName = command.OrganisationName,
            ReceiveNotifications = command.ReceiveNotifications,
            IsRegionalChair = false,
            Employer = new Employer
            {
                MemberId = command.MemberId,
                AccountId = command.AccountId,
                UserRef = command.UserRef
            },
            MemberProfiles = command.ProfileValues.Select(p => ProfileConverter(p, command.MemberId)).ToList(),
            MemberNotificationEventFormats = MapMemberNotificationEventFormats(command.MemberNotificationEventFormatValues, command.MemberId),
            MemberNotificationLocations = command.MemberNotificationLocationValues?
                .Select(p => MemberNotificationLocationsConverter(p, command.MemberId))
                .ToList(),
        };

        public static MemberProfile ProfileConverter(ProfileValue source, Guid memberId) => new() { MemberId = memberId, ProfileId = source.Id, ProfileValue = source.Value };
        public static MemberNotificationLocation MemberNotificationLocationsConverter(MemberNotificationLocationValues source, Guid memberId) => new() { MemberId = memberId, Name = source.Name, Radius = source.Radius, Latitude = source.Latitude, Longitude = source.Longitude };

        private static List<MemberNotificationEventFormat>? MapMemberNotificationEventFormats(IEnumerable<MemberNotificationEventFormatValues>? source, Guid memberId)
        {
            if (source == null)
            {
                return null;
            }

            // Check if "All" is present
            var eventTypeList = source.ToList();
            if (eventTypeList.Any(e => e.EventFormat == "All"))
            {
                // Replace "All" with the specific event types
                eventTypeList.Clear();
                eventTypeList.AddRange(new List<MemberNotificationEventFormatValues>
                {
                    new() { EventFormat = "InPerson", ReceiveNotifications = true },
                    new() { EventFormat = "Online", ReceiveNotifications = true },
                    new() { EventFormat = "Hybrid", ReceiveNotifications = true }
                });
            }

            return eventTypeList.Select(p => new MemberNotificationEventFormat
            {
                MemberId = memberId,
                EventFormat = p.EventFormat,
                ReceiveNotifications = p.ReceiveNotifications
            }).ToList();
        }
    }
}
