using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public record AttendanceModel(Guid CalendarEventId, EventFormat EventFormat, DateTime EventStartDate, string EventDescription)
{
    public static implicit operator AttendanceModel(Attendance source) =>
        new AttendanceModel(source.CalendarEventId, Enum.Parse<EventFormat>(source.CalendarEvent.EventFormat), source.CalendarEvent.StartDate, source.CalendarEvent.Description);
}
