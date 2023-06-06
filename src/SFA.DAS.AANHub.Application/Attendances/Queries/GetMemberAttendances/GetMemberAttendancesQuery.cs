using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.Attendances.Queries.GetMemberAttendances;

public record GetMemberAttendancesQuery(Guid MemberId, DateTime? FromDate, DateTime? ToDate)
    : IRequest<ValidatedResponse<GetMemberAttendancesQueryResult>>;

public record GetMemberAttendancesQueryResult(List<Attendance> Attendances);

public record Attendance(Guid CalendarEventId, EventFormat EventFormat, DateTime CalendarEventStartDate, string EventDescription);
