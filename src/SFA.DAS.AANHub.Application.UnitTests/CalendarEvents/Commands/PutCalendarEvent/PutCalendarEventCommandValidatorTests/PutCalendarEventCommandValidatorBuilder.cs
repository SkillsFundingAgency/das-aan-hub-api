using AutoFixture;
using Moq;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.PutCalendarEvent.PutCalendarEventCommandValidatorTests;

public class PutCalendarEventCommandValidatorBuilder
{
    public Mock<ICalendarsReadRepository> CalendarRepoMock { get; set; } = new();
    public Mock<IRegionsReadRepository> RegionRepoMock { get; set; } = new();
    public Mock<IMembersReadRepository> MembersRepoMock { get; set; } = new();
    public Mock<ICalendarEventsReadRepository> CalendarEventsRepoMock { get; set; } = new();

    public const string AdminActiveMemberId = "9278555b-5828-46b7-b7a1-8d07a4bbb6a5";

    public const string EventNotFoundId = "c7be8d81-b811-43f2-bd92-e48a2724d7d3";
    public const string EventIsNotActiveId = "d19b813b-b5f2-40f1-b9f7-fdaf488283b7";
    public const string EventIsInPastId = "b09c88f7-01b6-48fe-a755-e9591d0da729";

    public static PutCalendarEventCommandValidator Create() => new PutCalendarEventCommandValidatorBuilder()
        .AddEvents().Create();
}

public static class PutCalendarEventCommandValidatorBuilderExtensions
{
    public static Guid ToGuid(this string validGuid) => Guid.Parse(validGuid);

    public static PutCalendarEventCommandValidatorBuilder AddEvents(this PutCalendarEventCommandValidatorBuilder builder)
    {
        Fixture fixture = new();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        builder.CalendarEventsRepoMock.Setup(r => r.GetCalendarEvent(PutCalendarEventCommandValidatorBuilder.EventNotFoundId.ToGuid())).ReturnsAsync((CalendarEvent)null!);

        var calendarEventNotActive = fixture
            .Build<CalendarEvent>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.EventIsNotActiveId.ToGuid())
            .With(m => m.StartDate, DateTime.Today.AddDays(1))
            .With(m => m.IsActive, false)
            .Create();
        builder.CalendarEventsRepoMock.Setup(r => r.GetCalendarEvent(PutCalendarEventCommandValidatorBuilder.EventIsNotActiveId.ToGuid())).ReturnsAsync(calendarEventNotActive);

        var calendarEventInPast = fixture
           .Build<CalendarEvent>()
           .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.EventIsNotActiveId.ToGuid())
           .With(m => m.StartDate, DateTime.Today.AddDays(-1))
           .With(m => m.IsActive, true)
           .Create();
        builder.CalendarEventsRepoMock.Setup(r => r.GetCalendarEvent(PutCalendarEventCommandValidatorBuilder.EventIsInPastId.ToGuid())).ReturnsAsync(calendarEventInPast);

        return builder;
    }

    public static PutCalendarEventCommandValidator Create(this PutCalendarEventCommandValidatorBuilder builder) =>
        new PutCalendarEventCommandValidator(builder.CalendarRepoMock.Object, builder.RegionRepoMock.Object, builder.MembersRepoMock.Object, builder.CalendarEventsRepoMock.Object);

}
