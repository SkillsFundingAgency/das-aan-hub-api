using SFA.DAS.AANHub.Application.Models;

namespace SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
public class EventsModel
{
    public DateRangeModel EventsDateRange { get; set; } = null!;
    public List<EventAttendanceModel> Events { get; set; } = null!;
}
