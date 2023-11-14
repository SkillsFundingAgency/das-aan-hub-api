using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidatePlannedAttendees
{
    [TestCase(null, false, ErrorConstants.PlannedAttendeesMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.PlannedAttendeesMustBeValid)]
    [TestCase(1000001, false, ErrorConstants.PlannedAttendeesMustBeValid)]
    [TestCase(1, true, null)]
    public async Task Validate_PlannedAttendees_MustBeValidValue(int? plannedAttendees, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();

        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId),
            PlannedAttendees = plannedAttendees
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.PlannedAttendees);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.PlannedAttendees).WithErrorMessage(errorMessage);
        }
    }
}
