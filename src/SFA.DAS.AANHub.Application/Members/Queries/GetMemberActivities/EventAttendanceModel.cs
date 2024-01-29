namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class EventAttendanceModel
{
    public Guid CalendarEventId { get; set; }
    public DateTime EventDate { get; set; }
    public string EventTitle { get; set; } = null!;
    public long? Urn { get; set; }
}
