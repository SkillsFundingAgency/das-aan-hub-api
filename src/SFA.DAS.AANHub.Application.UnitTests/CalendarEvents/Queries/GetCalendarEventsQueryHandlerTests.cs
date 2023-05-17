using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CalendarEventsNotFound_ReturnsEmptyList(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        Member member)
    {
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id))
            .ReturnsAsync(() => new List<CalendarEventModel>());

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, 1), new CancellationToken());

        result.Result.CalendarEvents.Count.Should().Be(0);
        result.Result.TotalCount.Should().Be(0);
        result.Result.PageSize.Should().Be(Domain.Common.Constants.Pagination.PageSize);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_CalendarEventsFound_ReturnsCalendarEvents(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventsQueryHandler sut,
        CalendarEventModel calendarEvent,
        Member member)
    {
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvents(member.Id))
            .ReturnsAsync(new List<CalendarEventModel> { calendarEvent });

        var result = await sut.Handle(new GetCalendarEventsQuery(member.Id, 1), new CancellationToken());

        result.Result.CalendarEvents.Count.Should().Be(1);
        result.Result.TotalCount.Should().Be(1);
        result.Result.PageSize.Should().Be(Domain.Common.Constants.Pagination.PageSize);

        var calendarEvents = result.Result.CalendarEvents;

        calendarEvents.First().Should().BeEquivalentTo(calendarEvent);
    }
}
