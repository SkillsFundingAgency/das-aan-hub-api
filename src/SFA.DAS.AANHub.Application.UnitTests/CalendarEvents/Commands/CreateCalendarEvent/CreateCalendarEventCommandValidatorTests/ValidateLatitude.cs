using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;


namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateLatitude
{
    [TestCase(EventFormat.InPerson, null, false, ErrorConstants.LatitudeMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, -91, false, ErrorConstants.LatitudeMustBeValid)]
    [TestCase(EventFormat.InPerson, 91, false, ErrorConstants.LatitudeMustBeValid)]
    [TestCase(EventFormat.Hybrid, null, false, ErrorConstants.LatitudeMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, -91, false, ErrorConstants.LatitudeMustBeValid)]
    [TestCase(EventFormat.Hybrid, 91, false, ErrorConstants.LatitudeMustBeValid)]
    [TestCase(EventFormat.Online, 0, false, ErrorConstants.LatitudeMustBeEmpty)]
    [TestCase(EventFormat.InPerson, 0, true, null)]
    [TestCase(EventFormat.Hybrid, 0, true, null)]
    public async Task Validate_Latitude_MustBeValidValue(EventFormat eventFormat, double? latitude, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            EventFormat = eventFormat,
            Latitude = latitude
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Latitude);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Latitude).WithErrorMessage(errorMessage);
        }
    }
}
