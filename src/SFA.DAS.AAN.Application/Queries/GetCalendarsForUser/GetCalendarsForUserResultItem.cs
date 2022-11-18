
namespace SFA.DAS.AAN.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserResultItem
    {
        public string Calendar { get; set; }
        public long CalendarId { get; set; }
        public bool Create { get; set; }
        public bool Update { get; set; }
        public bool View { get; set; }
        public bool Delete { get; set; }
    }
}
