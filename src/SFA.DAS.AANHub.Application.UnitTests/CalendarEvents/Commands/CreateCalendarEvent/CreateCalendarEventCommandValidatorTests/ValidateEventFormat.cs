using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventFormat
{
    [TestCase(EventFormat.Online, true)]
    [TestCase(EventFormat.Hybrid, true)]
    [TestCase(EventFormat.InPerson, true)]
    [TestCase(null, false)]
    public async Task ShouldBeValidValue(EventFormat? eventFormat, bool isValid)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();
        CreateCalendarEventCommand command = new() { EventFormat = eventFormat };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.EventFormat);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.EventFormat);
        }
    }
}
