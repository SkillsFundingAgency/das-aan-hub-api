using MediatR;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
using SFA.DAS.AANHub.Application.Mediatr.Responses;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;

public class PutEventGuestsCommand : IRequest<ValidatedResponse<SuccessCommandResult>>, IAdminMemberId
{
    public Guid AdminMemberId { get; set; }
    public Guid CalendarEventId { get; set; }
    public CalendarEvent? CalendarEvent { get; set; }
    public IEnumerable<EventGuestModel> Guests { get; set; } = Enumerable.Empty<EventGuestModel>();
}
