using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateCalendarType
{
    [TestCase(1, true, null)]
    [TestCase(0, false, ErrorConstants.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(-1, false, ErrorConstants.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(6, false, ErrorConstants.CalendarTypeIdMustBeValid)]
    public async Task EventCalendarShouldBeValidValue(int calendarId, bool isValid, string? error)
    {
        PutCalendarEventCommand command = new() { AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId), CalendarId = calendarId };

        var sut = PutCalendarEventCommandValidatorBuilder.Create();

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
