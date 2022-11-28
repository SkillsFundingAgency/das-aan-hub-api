
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AAN.Application.Extensions;
using SFA.DAS.AAN.Domain.Entities;
using SFA.DAS.AAN.Domain.Interfaces;


namespace SFA.DAS.AAN.Application.Queries.GetCalendarEvents
{
    public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, IEnumerable<GetCalendarEventsResultItem>>
    {
        private readonly ICalendarsContext _calendarsContext;
        private readonly ICalendarEventsContext _calendarEventsContext;
        private readonly ICalendarPermissionsContext _calendarPermissionsContext;
        private readonly IMemberPermissionsContext _memberPermissionsContext;

        public GetCalendarEventsQueryHandler(ICalendarsContext calendarsContext,
            ICalendarEventsContext calendarEventsContext,
            ICalendarPermissionsContext calendarPermissionsContext,
            IMemberPermissionsContext memberPermissionsContext)
        {
            _calendarsContext = calendarsContext;
            _calendarEventsContext = calendarEventsContext;
            _calendarPermissionsContext = calendarPermissionsContext;
            _memberPermissionsContext = memberPermissionsContext;
        }

        public async Task<IEnumerable<GetCalendarEventsResultItem>> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Guid> eventIds = request.calendareventid.ToGuidList(",");
            IEnumerable<Int64> regionIds = request.region.ToIntList(",");

            IEnumerable<MemberPermission> memberPermissions =
                await _memberPermissionsContext.Entities
                                               .Where(m => m.MemberId == request.memberid && m.IsActive)
                                               .ToListAsync(cancellationToken);

            IQueryable<CalendarEvent> events =
                _calendarEventsContext.Entities
                                      .Where(x => x.CalendarId == request.calendarid);

            if (eventIds.Any())
                events = events.Where(x => eventIds.Contains(x.Id));

            if (regionIds.Any())
                events = events.Where(x => regionIds.Contains(x.RegionId));

            if (request.fromdate != null)
                events = events.Where(x => x.Start >= request.fromdate);

            if (request.todate != null)
                events = events.Where(x => x.End <= request.todate);

            IEnumerable<GetCalendarEventsResultItem> results =
                await events.Select(x => new GetCalendarEventsResultItem(x))
                            .ToListAsync(cancellationToken);

            return results;
        }
    }
}
