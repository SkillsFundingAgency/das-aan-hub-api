using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventByIdQueryResultTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_MapsFromCalendarEvent([Frozen] CalendarEvent source)
    {
        source.Attendees.ForEach(x => x.IsAttending = true);

        var sut = (GetCalendarEventByIdQueryResult)source;

        sut.Should().BeEquivalentTo(source, c => c.ExcludingMissingMembers());
        sut.CalendarEventId.Should().Be(source.Id);
    }
}
