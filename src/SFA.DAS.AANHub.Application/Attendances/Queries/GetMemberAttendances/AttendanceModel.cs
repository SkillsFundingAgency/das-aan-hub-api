using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public record AttendanceModel(Guid CalendarEventId, EventFormat EventFormat, DateTime CalendarEventStartDate, string EventDescription);
