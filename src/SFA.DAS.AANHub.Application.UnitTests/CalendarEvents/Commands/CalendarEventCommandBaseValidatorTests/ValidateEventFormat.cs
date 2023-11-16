using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class ValidateEventFormat
{
    [TestCase(EventFormat.Online, true)]
    [TestCase(EventFormat.Hybrid, true)]
    [TestCase(EventFormat.InPerson, true)]
    [TestCase(null, false)]
    public async Task Validate_EventFormat_ShouldBeValidValue(EventFormat? eventFormat, bool isValid)
    {
        var sut = CalendarEventCommandBaseValidatorBuilder.Create();
        CreateCalendarEventCommand command = new() { AdminMemberId = CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid(), EventFormat = eventFormat };

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
