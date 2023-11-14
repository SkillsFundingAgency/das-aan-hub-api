using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateContactEmail
{
    [TestCase(0, null, false, ErrorConstants.ContactEmailMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.ContactEmailMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.ContactEmailMustNotBeEmpty)]
    [TestCase(0, "invalid email", false, ErrorConstants.ContactEmailMustBeValid)]
    [TestCase(1, null, false, ErrorConstants.ContactEmailMustNotExceedLength)]
    [TestCase(0, "abc@mail.com", true, null)]
    public async Task Validate_ContactEmail_ShouldHaveValidValue(int length, string? contactEmail, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId),
            ContactEmail = length == 0 ? contactEmail : new string('a', 250) + "@mail.com"
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.ContactEmail);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.ContactEmail).WithErrorMessage(errorMessage);
        }
    }
}
