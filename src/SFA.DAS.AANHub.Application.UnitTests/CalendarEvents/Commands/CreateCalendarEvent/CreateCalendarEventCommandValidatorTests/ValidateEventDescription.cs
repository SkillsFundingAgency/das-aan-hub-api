using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventDescription
{
    [TestCase(0, null, false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.DescriptionMustNotBeEmpty)]
    [TestCase(0, "some valid Description ", true, null)]
    [TestCase(2001, "", false, ErrorConstants.DescriptionMustNotExceedLength)]
    public async Task ShouldHaveValidValue(int length, string? Description, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
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