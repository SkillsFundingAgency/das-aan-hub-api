using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateEventSummary
{
    [TestCase(0, null, false, ErrorConstants.SummaryMustNotBeEmpty)]
    [TestCase(0, "", false, ErrorConstants.SummaryMustNotBeEmpty)]
    [TestCase(0, " ", false, ErrorConstants.SummaryMustNotBeEmpty)]
    [TestCase(0, "some valid Summary ", true, null)]
    [TestCase(201, null, false, ErrorConstants.SummaryMustNotExceedLength)]
    public async Task Validate_EventSummary_ShouldBeValidValue(int length, string? Summary, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
            Summary = length == 0 ? Summary : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Summary);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Summary).WithErrorMessage(errorMessage);
        }
    }
}
