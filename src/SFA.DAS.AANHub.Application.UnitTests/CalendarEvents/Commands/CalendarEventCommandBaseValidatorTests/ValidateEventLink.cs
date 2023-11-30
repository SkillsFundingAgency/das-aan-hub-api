using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

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
    [TestCase(EventFormat.Hybrid, 0, "HTTPS://www.google.com", true, null)]
    [TestCase(EventFormat.Hybrid, 0, "HTTPS://WWW.google.com", true, null)]
    [TestCase(EventFormat.Online, 0, "https://test-meet.google.com", true, null)]
    [TestCase(EventFormat.Hybrid, 0, "https://teams.microsoft.com/l/meetup-join/19%3ameeting_ZGRmZC00NzlkLWFmZTktOTU4ZmFkMjA2ZDE1%thread.v2/0?context=%7b%22ad277c9-c60a-4da1-b5f3-b3b8Oid%22%3a%2209a4-c6-48-bc-ad60%22%7d", true, null)]
    public async Task Validate_EventLink_MustBeValidValue(EventFormat eventFormat, int length, string? eventLink, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
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
