using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;
public record EventGuestModel(Guid EventGuestId, string GuestName, string GuestJobTitle)
{
    public static implicit operator EventGuestModel(EventGuest source) => new(source.Id, source.GuestName, source.GuestJobTitle);
}
