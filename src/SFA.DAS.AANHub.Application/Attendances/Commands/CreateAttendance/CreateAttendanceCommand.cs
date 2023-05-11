using MediatR;
using SFA.DAS.AANHub.Application.Common.Validators.RequestedByMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Attendances.Commands.CreateAttendance;
public class CreateAttendanceCommand : IRequest<ValidatedResponse<CreateAttendanceCommandResponse>>, IRequestedByMemberId
{
    public Guid Id { get; set; }
    public Guid CalendarEventId { get; set; }
    public Guid MemberId { get; set; }
    public DateTime? AddedDate { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
    public bool Attended { get; set; }
    public int? OverallRating { get; set; }
    public DateTime? FeedbackCompletedDate { get; set; }

    public Guid RequestedByMemberId { get; set; }

    public CreateAttendanceCommand(Guid calendarEventId, Guid requestedByMemberId)
    {
        Id = Guid.NewGuid();
        CalendarEventId = calendarEventId;
        RequestedByMemberId = requestedByMemberId;
    }

    public static implicit operator Attendance(CreateAttendanceCommand command) => new()
    {
        Id = command.Id,
        CalendarEventId = command.CalendarEventId,
        MemberId = command.RequestedByMemberId,
        AddedDate = command.AddedDate,
        IsActive = command.IsActive,
        Attended = command.Attended,
        OverallRating = command.OverallRating,
        FeedbackCompletedDate = command.FeedbackCompletedDate,
    };
}
