using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateEventPostcode
{
    [TestCase(EventFormat.InPerson, null, false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, null, false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, "", false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, "", false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, " ", false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, " ", false, ErrorConstants.PostcodeMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, "invalid-value", false, ErrorConstants.PostcodeMustBeValid)]
    [TestCase(EventFormat.Hybrid, "xxxx 111", false, ErrorConstants.PostcodeMustBeValid)]
    [TestCase(EventFormat.Online, "CV1 2RJ", false, ErrorConstants.PostcodeMustBeEmpty)]
    [TestCase(EventFormat.InPerson, "CV1 2RJ", true, null)]
    [TestCase(EventFormat.Hybrid, "CV1 2RJ", true, null)]
    public async Task Validate_EventPostcode_ShouldBeValidValue(EventFormat eventFormat, string? postcode, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
            EventFormat = eventFormat,
            Postcode = postcode
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Postcode);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Postcode).WithErrorMessage(errorMessage);
        }
    }
}
