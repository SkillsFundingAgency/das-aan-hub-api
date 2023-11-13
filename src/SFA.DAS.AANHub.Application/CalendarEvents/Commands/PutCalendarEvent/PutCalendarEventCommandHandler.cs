using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
public class PutCalendarEventCommandHandler : IRequestHandler<PutCalendarEventCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly ICalendarEventsWriteRepository _calendarEventWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;

    public PutCalendarEventCommandHandler(ICalendarEventsWriteRepository calendarEventWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext)
    {
        _calendarEventWriteRepository = calendarEventWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PutCalendarEventCommand command, CancellationToken cancellationToken)
    {
        var existingEvent = await _calendarEventWriteRepository.GetCalendarEvent(command.CalendarEventId);

        CalendarEvent calendarEvent = command;
        calendarEvent.IsActive = existingEvent.IsActive;
        calendarEvent.CreatedDate = existingEvent.CreatedDate;
        calendarEvent.LastUpdatedDate = DateTime.UtcNow;



        //        See UpdateAttendance in PutAttendanceCommandHandler for pattern?

        //_calendarEventWriteRepository.Create(calendarEvent);
        //
        // _auditWriteRepository.Create(new Audit
        // {
        //     Action = "Create",
        //     ActionedBy = command.AdminMemberId,
        //     AuditTime = DateTime.UtcNow,
        //     After = JsonSerializer.Serialize(calendarEvent),
        //     Resource = nameof(CalendarEvent)
        // });

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }
}