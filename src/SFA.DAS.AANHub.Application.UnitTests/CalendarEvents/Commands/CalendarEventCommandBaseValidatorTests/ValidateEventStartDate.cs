using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateEventStartDate
{
    [TestCase(null, false, ErrorConstants.StartDateMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.StartDateMustBeInFuture)]
    [TestCase(-1, false, ErrorConstants.StartDateMustBeInFuture)]
    [TestCase(1, true, null)]
    public async Task Validate_EventStartDate_ShouldBeValidValue(int? mins, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
            StartDate = mins switch
            {
                0 => DateTime.MinValue,
                null => null,
                _ => DateTime.UtcNow.AddMinutes(mins.GetValueOrDefault())
            },
            EndDate = DateTime.UtcNow.AddDays(1)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.StartDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.StartDate).WithErrorMessage(errorMessage);
        }
    }

    [TestCase(0, true)]
    [TestCase(1, true)]
    [TestCase(-1, false)]
    public async Task ShouldBeLessThanEqualToEndDate(int addMinsToEndDate, bool isValid)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();
        var date = DateTime.UtcNow;
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
            StartDate = date,
            EndDate = date.AddMinutes(addMinsToEndDate)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.StartDate);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.StartDate).WithErrorMessage(ErrorConstants.StartDateMustBeLessThanEndDate);
        }
    }
}
