using SFA.DAS.AANHub.Application.Models;

namespace SFA.DAS.AANHub.Api.Models;

public record PutEventGuestsModel(IEnumerable<EventGuestModel> Guests);

