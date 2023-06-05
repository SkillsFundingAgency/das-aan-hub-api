﻿using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_PopulatesModelFromEntity(Guid memberId, DateTime? startDate, DateTime? toDate, List<EventFormat> eventFormat, List<int> calendarIds, List<int> regionIds, int page)
    {
        var model = new GetCalendarEventsQuery(memberId, startDate, toDate, eventFormat, calendarIds, regionIds, page);

        model.RequestedByMemberId.Should().Be(memberId);
        model.Page.Should().Be(page);
        model.EventFormats.Should().BeEquivalentTo(eventFormat);
    }
}
