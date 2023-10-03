using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateCalendarType
{
    [TestCase(1, true, null)]
    [TestCase(0, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(-1, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(6, false, CreateCalendarEventCommandValidator.CalendarTypeIdMustBeValid)]
    public async Task ShouldBeValidValue(int calendarId, bool isValid, string? error)
    {
        CreateCalendarEventCommand command = new() { CalendarId = calendarId };

        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        var result = await sut.TestValidateAsync(command);
        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.CalendarId);
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.CalendarId)
                .WithErrorMessage(error);
        }
    }
}
