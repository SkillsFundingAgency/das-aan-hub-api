namespace SFA.DAS.AANHub.Domain.Dtos;
public class CancelledAttendanceEventSummary
{
    public Guid CalendarEventId { get; set; }
    public Guid? AdminMemberId { get; set; }
    public string CalendarName { get; set; } = null!;
    public string EventFormat { get; set; } = null!;
    public string EventTitle { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TotalAmbassadorsCount { get; set; }
}
