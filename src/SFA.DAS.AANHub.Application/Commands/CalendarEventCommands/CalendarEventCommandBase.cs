
namespace SFA.DAS.AANHub.Application.Commands.CalendarEventCommands
{
    public class CalendarEventCommandBase
    {
        public Guid userid { get; set; }
        public long calendarid { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string? description { get; set; }
        public int regionid { get; set; }
        public string? location { get; set; }
        public string? postcode { get; set; }
        public string? eventlink { get; set; }
        public string? contact { get; set; }
        public string? email { get; set; }
    }
}
