using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateCalendarEventId
{
    [TestCase("00000000-0000-0000-0000-000000000000", false, ErrorConstants.EventIdNotSuppliedMessage)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EventNotFoundId, false, ErrorConstants.EventNotFoundMessage)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EventIsNotActiveId, false, ErrorConstants.EventNotActiveMessage)]
    [TestCase(PutCalendarEventCommandValidatorBuilder.EventIsInPastId, false, ErrorConstants.EventInPastMessage)]
    public async Task Validate_CalendarEventId_ShouldBeValidValue(string calendarEventId, bool isValid, string? errorMessage)
    {
        PutCalendarEventCommand command = new()
        {
            CalendarEventId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(calendarEventId),
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId)
        };
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.CalendarEventId);
            result.ShouldHaveAnyValidationError();
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(c => c.CalendarEventId)
                .WithErrorMessage(errorMessage);
        }
    }
}
