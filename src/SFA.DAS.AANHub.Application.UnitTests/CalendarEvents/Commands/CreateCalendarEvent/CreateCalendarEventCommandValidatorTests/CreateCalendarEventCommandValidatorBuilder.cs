using AutoFixture;
using Moq;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class CreateCalendarEventCommandValidatorBuilder
{
    public Mock<ICalendarsReadRepository> CalendarRepoMock { get; set; } = new();

    public static CreateCalendarEventCommandValidatorBuilder Build() => new();

    public static CreateCalendarEventCommandValidator Create() => new CreateCalendarEventCommandValidatorBuilder().AddCalendarData().Create();
}

public static class CreateCalendarEventCommandValidatorBuilderExtensions
{
    public static CreateCalendarEventCommandValidatorBuilder AddCalendarData(this CreateCalendarEventCommandValidatorBuilder builder)
    {
        Fixture fixture = new();
        var i = 1;
        var calendars = fixture
            .Build<Calendar>()
            .With(c => c.Id, () => i++)
            .CreateMany()
            .ToList();
        CancellationToken cancellationToken = new();
        builder.CalendarRepoMock.Setup(r => r.GetAllCalendars(cancellationToken)).ReturnsAsync(calendars);
        return builder;
    }

    public static CreateCalendarEventCommandValidator Create(this CreateCalendarEventCommandValidatorBuilder builder) =>
        new CreateCalendarEventCommandValidator(builder.CalendarRepoMock.Object);

}
