using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent;


public class PutCalendarEventCommandTests
{
    [Test, AutoData]
    public void Operator_ConvertsModelToCommand(PutCalendarEventModel sut)
    {
        PutCalendarEventCommand command = sut;
        command.Should().BeEquivalentTo(sut, options => options.ExcludingMissingMembers().Excluding(c => c.RegionId));
    }

    [TestCase(0, null)]
    [TestCase(1, 1)]
    public void Operator_ConvertsModelToCommand_RegionId(int regionId, int? expected)
    {
        var sut = new PutCalendarEventModel { RegionId = regionId };
        PutCalendarEventCommand command = sut;
        command.RegionId.Should().Be(expected);
    }
}