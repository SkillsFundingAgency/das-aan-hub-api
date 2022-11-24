using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;

namespace SFA.DAS.AANHub.Application.Queries.GetCalendars
{
    public class GetCalendarsQueryHandler : IRequestHandler<GetCalendarsQuery, IEnumerable<GetCalendarsResultItem>>
    {
        private readonly ICalendarsContext _calendarsContext;
        private readonly ICalendarPermissionsContext _calendarPermissionsContext;

        public GetCalendarsQueryHandler(ICalendarsContext calendarsContext, ICalendarPermissionsContext calendarPermissionsContext)
        {
            _calendarsContext = calendarsContext;
            _calendarPermissionsContext = calendarPermissionsContext;
        }

        public async Task<IEnumerable<GetCalendarsResultItem>> Handle(GetCalendarsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Calendar> calendars = await _calendarsContext.Entities.ToListAsync(cancellationToken);
            IEnumerable<CalendarPermission> permissions = await _calendarPermissionsContext.Entities.ToListAsync(cancellationToken);

            IEnumerable<GetCalendarsResultItem> items =
                calendars.Select(c => new GetCalendarsResultItem()
                {
                    Calendar = c.CalendarName,
                    CalendarId = c.Id,
                    Create = permissions.Where(x => x.CalendarId == c.Id && x.Create)
                                        .Select(x => x.PermissionId),
                    Update = permissions.Where(x => x.CalendarId == c.Id && x.Update)
                                        .Select(x => x.PermissionId),
                    View = permissions.Where(x => x.CalendarId == c.Id && x.View)
                                        .Select(x => x.PermissionId),
                    Delete = permissions.Where(x => x.CalendarId == c.Id && x.Delete)
                                        .Select(x => x.PermissionId)
                });

            return items;
        }
    }
}
