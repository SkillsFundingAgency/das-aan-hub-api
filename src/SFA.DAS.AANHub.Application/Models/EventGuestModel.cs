using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.Models;

public record EventGuestModel(string GuestName, string GuestJobTitle)
{
    public static implicit operator EventGuestModel(EventGuest source) => new(source.GuestName, source.GuestJobTitle);
}
