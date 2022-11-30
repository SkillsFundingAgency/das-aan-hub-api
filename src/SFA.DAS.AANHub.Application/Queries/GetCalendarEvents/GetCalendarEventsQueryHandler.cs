
using MediatR;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.AANHub.Application.Extensions;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;


namespace SFA.DAS.AANHub.Application.Queries.GetCalendarEvents
{
    public class GetCalendarEventsQueryHandler : IRequestHandler<GetCalendarEventsQuery, IEnumerable<GetCalendarEventsResultItem>>
    {
        private readonly ICalendarEventsContext _calendarEventsContext;

        public GetCalendarEventsQueryHandler(ICalendarEventsContext calendarEventsContext)
        {
            _calendarEventsContext = calendarEventsContext;
        }

        public async Task<IEnumerable<GetCalendarEventsResultItem>> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<Guid> eventIds = request.calendareventid.ToGuidList(",");
            IEnumerable<Int64> regionIds = request.region.ToIntList(",");

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
