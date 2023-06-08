using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Calendars.Queries.GetCalendars;

public class GetCalendarsQueryResult
{
    public IEnumerable<CalendarModel> Calendars { get; set; } = null!;

    public static implicit operator GetCalendarsQueryResult(List<Calendar> calendars) => new()
    {
        Calendars = calendars.Select(r => new CalendarModel(r.Id, r.CalendarName, r.EffectiveFromDate, r.EffectiveToDate, r.Ordering))
    };
}

public record CalendarModel(int Id, string CalendarName, DateTime EffectiveFrom, DateTime? EffectiveTo, int Ordering);
