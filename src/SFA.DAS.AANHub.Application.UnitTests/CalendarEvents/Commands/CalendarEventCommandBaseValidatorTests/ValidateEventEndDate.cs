﻿using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateEventEndDate
{
    [TestCase(null, false, ErrorConstants.EndDateMustNotBeEmpty)]
    [TestCase(0, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(-1, false, ErrorConstants.EndDateMustBeInFuture)]
    [TestCase(1, true, null)]
    public async Task Validate_EventEndDate_ShouldBeValidValue(int? mins, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
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
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();
        var date = DateTime.UtcNow;
        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
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