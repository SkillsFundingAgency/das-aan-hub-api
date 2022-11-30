using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserQueryHandler : IRequestHandler<GetCalendarsForUserQuery, GetCalendarsForUserResult>
    {
        private readonly ICalendarsContext _calendarsContext;
        private readonly ICalendarPermissionsContext _calendarPermissionsContext;
        private readonly IMemberPermissionsContext _memberPermissionsContext;

        public GetCalendarsForUserQueryHandler(ICalendarsContext calendarsContext,
            ICalendarPermissionsContext calendarPermissionsContext,
            IMemberPermissionsContext memberPermissionsContext)
        {
            _calendarsContext = calendarsContext;
            _calendarPermissionsContext = calendarPermissionsContext;
            _memberPermissionsContext = memberPermissionsContext;
        }

        public async Task<GetCalendarsForUserResult> Handle(GetCalendarsForUserQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<long> permissionIds =
                await _memberPermissionsContext.Entities
                                               .Where(m => m.MemberId == request.MemberId && m.IsActive)
                                               .Select(m => m.PermissionId)
                                               .Distinct()
                                               .ToListAsync(cancellationToken);
            IEnumerable<CalendarPermission> calendarPermissions =
                await _calendarPermissionsContext.Entities
                                                 .Where(cp => permissionIds.Contains(cp.PermissionId))
                                                 .ToListAsync(cancellationToken);
            var calendarIds = calendarPermissions.Select(cp => cp.CalendarId)
                                                               .Distinct();
            IEnumerable<Calendar> calendars =
                await _calendarsContext.Entities
                                       .Where(c => calendarIds.Contains(c.Id))
                                       .ToListAsync(cancellationToken);


            var result = new GetCalendarsForUserResult()
            {
                Permissions = permissionIds,
                Calendars = calendars.Select(c => new GetCalendarsForUserResultItem()
                {
                    Calendar = c.CalendarName,
                    CalendarId = c.Id,
                    Create = calendarPermissions.Any(cp => cp.CalendarId == c.Id && cp.Create),
                    Update = calendarPermissions.Any(cp => cp.CalendarId == c.Id && cp.Update),
                    View = calendarPermissions.Any(cp => cp.CalendarId == c.Id && cp.View),
                    Delete = calendarPermissions.Any(cp => cp.CalendarId == c.Id && cp.Delete),
                })
            };

            return result;
        }
    }
}
