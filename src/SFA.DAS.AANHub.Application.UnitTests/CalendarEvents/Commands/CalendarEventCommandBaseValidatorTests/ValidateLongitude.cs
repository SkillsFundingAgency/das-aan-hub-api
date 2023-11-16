using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CalendarEventCommandBase.CalendarEventCommandBaseValidator;


namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateLongitude
{
    [TestCase(EventFormat.InPerson, null, false, ErrorConstants.LongitudeMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, -181, false, ErrorConstants.LongitudeMustBeValid)]
    [TestCase(EventFormat.InPerson, 181, false, ErrorConstants.LongitudeMustBeValid)]
    [TestCase(EventFormat.Hybrid, null, false, ErrorConstants.LongitudeMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, -181, false, ErrorConstants.LongitudeMustBeValid)]
    [TestCase(EventFormat.Hybrid, 181, false, ErrorConstants.LongitudeMustBeValid)]
    [TestCase(EventFormat.Online, 0, false, ErrorConstants.LongitudeMustBeEmpty)]
    [TestCase(EventFormat.InPerson, 0, true, null)]
    [TestCase(EventFormat.Hybrid, 0, true, null)]
    public async Task Validate_Longitude_MustBeValidValue(EventFormat eventFormat, double? longitude, bool isValid, string? errorMessage)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(),
            EventFormat = eventFormat,
            Longitude = longitude
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Longitude);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Longitude).WithErrorMessage(errorMessage);
        }
    }
}
