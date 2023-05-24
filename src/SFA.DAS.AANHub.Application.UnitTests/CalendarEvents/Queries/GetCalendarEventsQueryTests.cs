using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_PopulatesModelFromEntity(Guid memberId, DateTime? startDate, DateTime? endDate, int page)
    {
        var model = new GetCalendarEventsQuery(memberId, startDate, endDate, page);

        model.RequestedByMemberId.Should().Be(memberId);
        model.Page.Should().Be(page);
    }
}
