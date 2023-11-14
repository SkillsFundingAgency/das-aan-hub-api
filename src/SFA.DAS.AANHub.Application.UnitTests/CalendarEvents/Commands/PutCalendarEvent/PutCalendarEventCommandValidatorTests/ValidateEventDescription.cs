using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateEventDescription
{
    [TestCase(0, null, false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, "some valid Description ", true, null)]
    [TestCase(2001, null, false, ErrorConstants.DescriptionMustNotExceedLength)]
    public async Task Validate_EventDescription_ShouldHaveValidValue(int length, string? Description, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId),
            Description = length == 0 ? Description : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Description);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Description).WithErrorMessage(errorMessage);
        }
    }
}
