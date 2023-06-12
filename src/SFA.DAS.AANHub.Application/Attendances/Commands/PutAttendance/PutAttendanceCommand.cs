using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IRequestedByMemberId
{
    public Guid CalendarEventId { get; set; }
    public Guid RequestedByMemberId { get; set; }
   
    public bool IsAttending { get; set; }

    public PutAttendanceCommand(Guid calendarEventId, Guid requestedByMemberId, bool isAttending)
    {
        CalendarEventId = calendarEventId;
        RequestedByMemberId = requestedByMemberId;
        IsAttending = isAttending;
    }

    public static implicit operator Attendance(PutAttendanceCommand command) 
    {
        return new()
        {
            Id = Guid.NewGuid(),
            CalendarEventId = command.CalendarEventId,
            MemberId = command.RequestedByMemberId,
            AddedDate = DateTime.UtcNow,
            IsAttending = command.IsAttending,
        };
    }
}
