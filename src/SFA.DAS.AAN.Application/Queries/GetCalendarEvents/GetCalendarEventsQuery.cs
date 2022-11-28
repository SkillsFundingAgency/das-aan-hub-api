
using MediatR;


namespace SFA.DAS.AAN.Application.Queries.GetCalendarEvents
{
    public class GetCalendarEventsQuery : IRequest<IEnumerable<GetCalendarEventsResultItem>>
    {
        public Guid memberid { get; set; }
        public long calendarid { get; set; }
        public string? calendareventid { get; set; }
        public string? region { get; set; }
        public DateTime? fromdate { get; set; }
        public DateTime? todate { get; set; }
    }
}
