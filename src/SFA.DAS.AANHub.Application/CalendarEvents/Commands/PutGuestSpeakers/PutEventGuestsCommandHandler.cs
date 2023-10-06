using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using System.Text.Json;

namespace SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutGuestSpeakers;
internal class PutEventGuestsCommandHandler : IRequestHandler<PutEventGuestsCommand, ValidatedResponse<SuccessCommandResult>>
{
    private readonly IEventGuestsWriteRepository _eventGuestsWriteRepository;
    private readonly IAuditWriteRepository _auditWriteRepository;
    private readonly IAanDataContext _aanDataContext;

    public PutEventGuestsCommandHandler(IEventGuestsWriteRepository eventGuestsWriteRepository, IAuditWriteRepository auditWriteRepository, IAanDataContext aanDataContext)
    {
        _eventGuestsWriteRepository = eventGuestsWriteRepository;
        _auditWriteRepository = auditWriteRepository;
        _aanDataContext = aanDataContext;
    }

    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(PutEventGuestsCommand request, CancellationToken cancellationToken)
    {
        var guests = request.Guests.Select(x => new EventGuest { GuestName = x.GuestName, GuestJobTitle = x.GuestJobTitle, CalendarEventId = request.CalendarEventId }).ToList();
        _eventGuestsWriteRepository.DeleteAll(request.CalendarEventId);
        _eventGuestsWriteRepository.CreateGuests(guests);

        _auditWriteRepository.Create(new Audit
        {
            Action = "Put",
            ActionedBy = request.AdminMemberId,
            AuditTime = DateTime.UtcNow,
            Before = JsonSerializer.Serialize(request.CalendarEvent?.EventGuests),
            After = JsonSerializer.Serialize(guests),
            Resource = nameof(EventGuest)
        });

        await _aanDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>(new SuccessCommandResult());
    }
}
