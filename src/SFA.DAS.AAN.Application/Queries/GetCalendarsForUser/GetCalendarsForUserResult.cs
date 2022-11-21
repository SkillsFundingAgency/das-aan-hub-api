
namespace SFA.DAS.AAN.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserResult
    {
        public IEnumerable<long>? Permissions { get; set; }
        public IEnumerable<GetCalendarsForUserResultItem> Calendars { get; set; }
    }
}
