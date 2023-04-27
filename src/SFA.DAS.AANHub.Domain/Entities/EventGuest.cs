namespace SFA.DAS.AANHub.Domain.Entities;

public class EventGuest
{
    public Guid Id { get; set; }
    public Guid CalendarEventId { get; set; }
    public string GuestName { get; set; } = string.Empty;
    public string GuestJobTitle { get; set; } = string.Empty;

    public CalendarEvent CalendarEvent { get; set; } = null!;
}