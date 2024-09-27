using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class EventAttendanceCancelEmailTemplate
{
    [JsonPropertyName("calendarEventId")]
    public string CalendarEventId { get; set; } = null!;
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = null!;
    [JsonPropertyName("eventformat")]
    public string EventFormat { get; set; } = null!;
    [JsonPropertyName("eventname")]
    public string EventName { get; set; } = null!;
    [JsonPropertyName("contact")]
    public string Contact { get; set; } = null!;
    [JsonPropertyName("datetime")]
    public string Datetime { get; set; } = null!;
    [JsonPropertyName("totalambassadors")]
    public int TotalAmbassadorsCount { get; set; }
}
