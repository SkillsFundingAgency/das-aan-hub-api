using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventLink
{
    [TestCase(EventFormat.InPerson, 0, "https://www.google.com", false, ErrorConstants.EventLinkMustBeEmpty)]
    [TestCase(EventFormat.Hybrid, 0, "invalid link", false, ErrorConstants.EventLinkMustBeValid)]
    [TestCase(EventFormat.Online, 0, "invalid link", false, ErrorConstants.EventLinkMustBeValid)]
    [TestCase(EventFormat.Online, 2001, null, false, ErrorConstants.EventLinkMustNotExceedLength)]
    [TestCase(EventFormat.Hybrid, 2001, null, false, ErrorConstants.EventLinkMustNotExceedLength)]
    [TestCase(EventFormat.Hybrid, 0, null, true, null)]
    [TestCase(EventFormat.Online, 0, null, true, null)]
    [TestCase(EventFormat.Online, 0, "https://www.google.com", true, null)]
    [TestCase(EventFormat.Hybrid, 0, "https://www.google.com", true, null)]
    public async Task Validate_EventLink_MustBeValidValue(EventFormat eventFormat, int length, string? eventLink, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
            EventFormat = eventFormat,
            EventLink = length == 0 ? eventLink : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EventLink);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.EventLink).WithErrorMessage(errorMessage);
        }
    }
}
