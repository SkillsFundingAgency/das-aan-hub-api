using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventEndDate
{
    [TestCase(null, false, ErrorConstants.EndDateMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(-1, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(1, true, null)]
    public async Task Validate_EventEndDate_ShouldBeValidValue(int? mins, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
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
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();
        var date = DateTime.UtcNow;
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
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
            result.ShouldHaveValidationErrorFor(c => c.EndDate).WithErrorMessage(ErrorConstants.EndDateMustBeLessThanEndDate);
        }
    }
}