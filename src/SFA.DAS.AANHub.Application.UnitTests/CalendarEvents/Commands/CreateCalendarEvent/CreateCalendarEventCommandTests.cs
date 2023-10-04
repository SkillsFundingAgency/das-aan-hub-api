using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent;

public class CreateCalendarEventCommandTests
{
    [Test, AutoData]
    public void Operator_ConvertsToEntity(CreateCalendarEventCommand sut)
    {
        CalendarEvent calendarEvent = sut;

        calendarEvent.Should().BeEquivalentTo(sut, options => options.ExcludingMissingMembers().Excluding(c => c.EventFormat));
        calendarEvent.EventFormat.Should().Be(sut.EventFormat.ToString());
    }
}
