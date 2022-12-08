
namespace SFA.DAS.AANHub.Application.Queries.GetCalendarsForUser
{
    public class GetCalendarsForUserResultItem
    {
        public string? Calendar { get; set; }
        public long CalendarId { get; set; }
        public bool HasCreate { get; set; }
        public bool HasUpdate { get; set; }
        public bool HasView { get; set; }
        public bool HasDelete { get; set; }
    }
}
