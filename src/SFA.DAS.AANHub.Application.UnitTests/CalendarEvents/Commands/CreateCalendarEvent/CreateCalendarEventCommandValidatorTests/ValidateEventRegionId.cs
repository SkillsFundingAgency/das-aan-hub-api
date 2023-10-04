using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class ValidateEventRegionId
{
    [TestCase(null, true)]
    [TestCase(1, true)]
    [TestCase(99, false)]
    public async Task Validate_EventRegionId_ShouldBeValidValue(int? regionId, bool isValid)
    {
        var sut = CreateCalendarEventCommandValidatorBuilder.Create();

        CreateCalendarEventCommand command = new()
        {
            RegionId = regionId
        };

        var result = await sut.TestValidateAsync(command);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(c => c.RegionId);
        }
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.RegionId);
        }
    }
}
