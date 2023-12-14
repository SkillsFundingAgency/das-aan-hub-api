using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;

public class GetCalendarEventsModelTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_ConvertModelToQuery(GetCalendarEventsModel source)
    {
        GetCalendarEventsQuery sut = source;

        sut.Should().BeEquivalentTo(source, s => s.ExcludingMissingMembers());
    }

    [Test, RecursiveMoqAutoData]
    public void Operator_ConvertModelToQuery_CheckRenamedFields(GetCalendarEventsModel source)
    {
        GetCalendarEventsQuery sut = source;

        sut.RegionIds.Should().BeEquivalentTo(source.RegionId);
        sut.EventFormats.Should().BeEquivalentTo(source.EventFormat);
        sut.CalendarIds.Should().BeEquivalentTo(source.CalendarId);
    }
}
