
namespace SFA.DAS.AAN.Application.Queries.GetCalendars
{
    public class GetCalendarsResultItem
    {
        public string Calendar { get; set; }
        public long CalendarId { get; set; }
        public IEnumerable<long>? Create { get; set; }
        public IEnumerable<long>? Update { get; set; }
        public IEnumerable<long>? View { get; set; }
        public IEnumerable<long>? Delete { get; set; }
    }
}
