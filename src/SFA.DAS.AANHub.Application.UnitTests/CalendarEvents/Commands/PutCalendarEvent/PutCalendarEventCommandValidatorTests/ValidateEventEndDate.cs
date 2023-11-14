using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class ValidateEventEndDate
{
    [TestCase(null, false, ErrorConstants.EndDateMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(-1, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(1, true, null)]
    public async Task Validate_EventEndDate_ShouldBeValidValue(int? mins, bool isValid, string? errorMessage)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();
        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId),
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = mins switch
            {
                0 => DateTime.MinValue,
                null => null,
                _ => DateTime.UtcNow.AddMinutes(mins.GetValueOrDefault())
            }
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EndDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.EndDate).WithErrorMessage(errorMessage);
        }
    }

    [TestCase(0, true)]
    [TestCase(-1, true)]
    [TestCase(1, false)]
    public async Task ShouldBeGreaterThanEqualToStartDate(int addMinsToStartDate, bool isValid)
    {
        var sut = PutCalendarEventCommandValidatorBuilder.Create();
        var date = DateTime.UtcNow;
        PutCalendarEventCommand command = new()
        {
            AdminMemberId = PutCalendarEventCommandValidatorBuilderExtensions.ToGuid(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId),
            StartDate = date.AddMinutes(addMinsToStartDate),
            EndDate = date
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EndDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.EndDate).WithErrorMessage(ErrorConstants.EndDateMustBeLessThanStartDate);
        }
    }
}