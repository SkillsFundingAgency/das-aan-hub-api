using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Entities;

public class Attendance
{
    public Guid Id { get; set; }
    public Guid CalendarEventId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime AddedDate { get; set; }
    public bool IsAttending { get; set; }


    [JsonIgnore]
    public CalendarEvent CalendarEvent { get; set; } = null!;

    [JsonIgnore]
    public Member Member { get; set; } = null!;
}
