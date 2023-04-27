namespace SFA.DAS.AANHub.Domain.Entities;

public class EventGuest
{
    public Guid Id { get; set; }
    public Guid CalendarEventId { get; set; }
    public string GuestName { get; set; }
    public string GuestJobTitle { get; set; }

    public CalendarEvent CalendarEvent { get; set; } = null!;
}