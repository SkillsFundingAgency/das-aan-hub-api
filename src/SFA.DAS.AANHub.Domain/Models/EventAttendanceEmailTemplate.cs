using System.Text.Json.Serialization;

namespace SFA.DAS.AANHub.Domain.Models;
public class EventAttendanceEmailTemplate
{
    public EventAttendanceEmailTemplate(string firstName, string lastName, string eventName, string date, string time)
    {
        Contact = $"{firstName} {lastName}";
        EventName = eventName;
        Date = date;
        Time = time;
    }

    [JsonPropertyName("contact")]
    public string Contact { get; set; }

    [JsonPropertyName("eventname")]
    public string EventName { get; set; }

    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }
}
