using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidatePlannedAttendees
{
    [TestCase(null, false, ErrorConstants.PlannedAttendeesMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.PlannedAttendeesMustBeValid)]
    [TestCase(1000001, false, ErrorConstants.PlannedAttendeesMustBeValid)]
    [TestCase(1, true, null)]
    public async Task Validate_PlannedAttendees_MustBeValidValue(int? plannedAttendees, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
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
