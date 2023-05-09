using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Queries;

public class GetCalendarEventByIdQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnsNull_IfCalendarEventId_DoesNotExist(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        GetCalendarEventByIdQueryHandler sut,
        CalendarEvent nonExistentCalendarEvent,
        Member member)
    {
        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvent(nonExistentCalendarEvent.Id))
                                        .ReturnsAsync(() => null);

        var result = await sut.Handle(new GetCalendarEventByIdQuery(nonExistentCalendarEvent.Id, member.Id), new CancellationToken());

        result.Result.Should().BeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_ReturnsCalendarEvent_IfCalendarEventId_AndRequestedUserId_Exist(
        [Frozen] Mock<ICalendarEventsReadRepository> calendarEventsReadRepositoryMock,
        [Frozen] Mock<IMembersReadRepository> membersReadRepositoryMock,
        GetCalendarEventByIdQueryHandler sut,
        CalendarEvent calendarEvent,
        Member member)
    {
        membersReadRepositoryMock.Setup(m => m.GetMember(member.Id))
                                 .ReturnsAsync(member);

        calendarEventsReadRepositoryMock.Setup(c => c.GetCalendarEvent(calendarEvent.Id))
                                        .ReturnsAsync(calendarEvent);

        var result = await sut.Handle(new GetCalendarEventByIdQuery(calendarEvent.Id, member.Id), new CancellationToken());

        result.Result.CalendarEvent.Id.Should().Be(calendarEvent.Id);
    }
}
