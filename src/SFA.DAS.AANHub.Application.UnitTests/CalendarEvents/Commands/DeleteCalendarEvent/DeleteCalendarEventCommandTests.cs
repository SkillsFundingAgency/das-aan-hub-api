using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.DeleteCalendarEvent;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.DeleteCalendarEvent;

public class DeleteCalendarEventCommandTests
{
    [Test, AutoData]
    public void Constructor_SetsUpCommand(Guid calendarEventId, Guid memberId)
    {
        var command = new DeleteCalendarEventCommand(calendarEventId, memberId);

        command.CalendarEventId.Should().Be(calendarEventId);
        command.RequestedByMemberId.Should().Be(memberId);
    }
}
