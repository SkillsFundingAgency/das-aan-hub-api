﻿using FluentValidation;
using SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
using SFA.DAS.AANHub.Application.Models;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.EventGuests.PutEventGuests;

public class PutEventGuestsCommandValidator : AbstractValidator<PutEventGuestsCommand>
{
    public const string CalendarEventDoesNotExist = "Calendar event does not exist";
    public const string CalendarEventIsNotActive = "Cannot amend a calendar event that has been cancelled";
    public const string CalendarEventIsInPast = "Cannot amend a calendar event that is in the past";

    public const string GuestNamesAndJobTitlesMustBePresent =
        "One or more of the guest speakers has a missing name or job title and organisation";

    public PutEventGuestsCommandValidator(IMembersReadRepository membersReadRepository)
    {
        Include(new AdminMemberIdValidator(membersReadRepository));

        RuleFor(c => c.CalendarEvent)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(CalendarEventDoesNotExist)
            .Must(calendarEvent => calendarEvent!.IsActive)
            .WithMessage(CalendarEventIsNotActive)
            .Must(calendarEvent => calendarEvent!.StartDate > DateTime.UtcNow)
            .WithMessage(CalendarEventIsInPast);

        RuleFor(c => c.Guests)
            .Must(GuestNamesAndJobTitlesComplete)
            .WithMessage(GuestNamesAndJobTitlesMustBePresent);
    }

    private bool GuestNamesAndJobTitlesComplete(PutEventGuestsCommand command, IEnumerable<EventGuestModel> guests)
    {

        foreach (var guest in command.Guests)
        {
            if (string.IsNullOrEmpty(guest.GuestName) || string.IsNullOrEmpty(guest.GuestJobTitle))
            {
                return false;
            }
        }

        return true;
    }
}
