using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateContactName
{
    [TestCase(0, null, false, ErrorConstants.ContactNameMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.ContactNameMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.ContactNameMustNotBeEmpty)]
    [TestCase(201, null, false, ErrorConstants.ContactNameMustNotExceedLength)]
    [TestCase(0, "some valid ContactName ", true, null)]
    public async Task Validate_EventContactName_ShouldHaveValidValue(int length, string? contactName, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
            ContactName = length == 0 ? contactName : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.ContactName);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.ContactName).WithErrorMessage(errorMessage);
        }
    }
}
