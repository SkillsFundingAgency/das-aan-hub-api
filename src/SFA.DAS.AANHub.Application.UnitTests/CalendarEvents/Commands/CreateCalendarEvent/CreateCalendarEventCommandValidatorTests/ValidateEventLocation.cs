using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using ErrorConstants = SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidator;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventLocation
{
    [TestCase(EventFormat.InPerson, 0, null, false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, 0, "", false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, 0, " ", false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.InPerson, 0, "some valid Location ", true, null)]
    [TestCase(EventFormat.InPerson, 201, null, false, ErrorConstants.LocationMustNotExceedLength)]
    [TestCase(EventFormat.Hybrid, 0, null, false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, 0, null, false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, 0, "", false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, 0, " ", false, ErrorConstants.LocationMustNotBeEmpty)]
    [TestCase(EventFormat.Hybrid, 0, "some valid Location ", true, null)]
    [TestCase(EventFormat.Hybrid, 201, null, false, ErrorConstants.LocationMustNotExceedLength)]
    [TestCase(EventFormat.Online, 0, "valid Location", false, ErrorConstants.LocationMustBeEmpty)]
    public async Task Validate_EventLocation_ShouldBeValidValue(EventFormat eventFormat, int length, string? Location, bool isValid, string? errorMessage)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            AdminMemberId = CreateCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid(),
            EventFormat = eventFormat,
            Location = length == 0 ? Location : new string('a', length)
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Location);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Location).WithErrorMessage(errorMessage);
        }
    }
}
