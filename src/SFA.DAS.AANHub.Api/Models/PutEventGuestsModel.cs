namespace SFA.DAS.AANHub.Api.Models;

public record PutEventGuestsModel(List<Guest> Guests);

public record Guest(string? GuestName, string? GuestJobTitle);
