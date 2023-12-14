using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Api.Models;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Models;

public class GetCalendarEventsModelTests
{
    [Test]
    [RecursiveMoqAutoData]
    public void Operator_ConvertModelToQuery(GetCalendarEventsModel source)
    {
        GetCalendarEventsQuery sut = source;

        sut.Should().BeEquivalentTo(source, s => s.ExcludingMissingMembers());

        //
        // Expectation has property sut.EventFormat that the other object does not have.
        //     Expectation has property sut.CalendarId that the other object does not have.
        //     Expectation has property sut.RegionId that the other object does not have.

    }
}
