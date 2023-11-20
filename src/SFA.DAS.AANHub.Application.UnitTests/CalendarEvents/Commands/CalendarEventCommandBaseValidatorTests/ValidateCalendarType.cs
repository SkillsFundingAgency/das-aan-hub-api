using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateCalendarType
{
    [TestCase(1, true, null)]
    [TestCase(0, false, ErrorConstants.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(-1, false, ErrorConstants.CalendarTypeIdMustNotBeEmpty)]
    [TestCase(6, false, ErrorConstants.CalendarTypeIdMustBeValid)]
    public async Task EventCalendarShouldBeValidValue(int calendarId, bool isValid, string? error)
    {
        CreateCalendarEventCommand command = new() { AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(), CalendarId = calendarId };

        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

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
