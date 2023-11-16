using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateCalendarEventId
{
    [TestCase("00000000-0000-0000-0000-000000000000", false, ErrorConstants.EventIdNotSuppliedMessage)]
    [TestCase("c7be8d81-b811-43f2-bd92-e48a2724d7d3", false, ErrorConstants.EventNotFoundMessage)]
    [TestCase("d19b813b-b5f2-40f1-b9f7-fdaf488283b7", false, ErrorConstants.EventNotActiveMessage)]
    [TestCase("b09c88f7-01b6-48fe-a755-e9591d0da729", false, ErrorConstants.EventInPastMessage)]
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
