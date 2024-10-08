﻿using MediatR;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandHandler : IRequestHandler<CreateCalendarEventCommand, ValidatedResponse<CreateCalendarEventCommandResult>>
{
    private readonly ICalendarEventsWriteRepository _calendarEventWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;

    public CreateCalendarEventCommandHandler(ICalendarEventsWriteRepository calendarEventWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext)
    {
        _calendarEventWriteRepository = calendarEventWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
    }

    public async Task<ValidatedResponse<CreateCalendarEventCommandResult>> Handle(CreateCalendarEventCommand request, CancellationToken cancellationToken)
    {
        CalendarEvent calendarEvent = request;
        calendarEvent.IsActive = true;
        calendarEvent.CreatedDate = DateTime.UtcNow;
        calendarEvent.LastUpdatedDate = DateTime.UtcNow;
        calendarEvent.CreatedByMemberId = request.AdminMemberId;

        _calendarEventWriteRepository.Create(calendarEvent);

        _auditWriteRepository.Create(new Audit
        {
            Action = AuditAction.Create,
            ActionedBy = request.AdminMemberId,
            AuditTime = DateTime.UtcNow,
            After = JsonSerializer.Serialize(calendarEvent),
            Resource = nameof(CalendarEvent),
            EntityId = calendarEvent.Id
        });

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<CreateCalendarEventCommandResult>(new CreateCalendarEventCommandResult(calendarEvent.Id));
    }
}
