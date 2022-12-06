using MediatR;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserQueryHandler : IRequestHandler<GetCalendarsForUserQuery, GetCalendarsForUserResult>
    {
        private readonly ICalendarsReadRepository _calendarsReadRepository;
        private readonly ICalendarsPermissionsReadRepository _calendarsPermissionsReadRepository;
        private readonly IMembersPermissionsReadRepository _membersPermissionsReadRepository;

        public GetCalendarsForUserQueryHandler(ICalendarsReadRepository calendarsReadRepository,
            ICalendarsPermissionsReadRepository calendarsPermissionsReadRepository,
            IMembersPermissionsReadRepository membersPermissionsReadRepository)
        {
            _calendarsReadRepository = calendarsReadRepository;
            _calendarsPermissionsReadRepository = calendarsPermissionsReadRepository;
            _membersPermissionsReadRepository = membersPermissionsReadRepository;
        }

        public async Task<GetCalendarsForUserResult> Handle(GetCalendarsForUserQuery request, CancellationToken cancellationToken)
        {
            var permissionIds = await _membersPermissionsReadRepository.GetAllMemberPermissionsForUser(request.MemberId);
            var calendarPermissionsForUser = await _calendarsPermissionsReadRepository.GetAllCalendarsPermissionsForUser(request.MemberId);
            var calendarIds = calendarPermissionsForUser.Select(cp => cp.CalendarId)
                                                               .Distinct();
            var calendars = _calendarsReadRepository.GetAllCalendars().Result.Where(c => calendarIds.Contains(c.Id));


            var result = new GetCalendarsForUserResult()
            {
                Permissions = permissionIds,
                Calendars = calendars.Select(c => new GetCalendarsForUserResultItem()
                {
                    Calendar = c.CalendarName,
                    CalendarId = c.Id,
                    Create = calendarPermissionsForUser.Any(cp => cp.CalendarId == c.Id && cp.Create),
                    Update = calendarPermissionsForUser.Any(cp => cp.CalendarId == c.Id && cp.Update),
                    View = calendarPermissionsForUser.Any(cp => cp.CalendarId == c.Id && cp.View),
                    Delete = calendarPermissionsForUser.Any(cp => cp.CalendarId == c.Id && cp.Delete),
                })
            };

            return result;
        }
    }
}
