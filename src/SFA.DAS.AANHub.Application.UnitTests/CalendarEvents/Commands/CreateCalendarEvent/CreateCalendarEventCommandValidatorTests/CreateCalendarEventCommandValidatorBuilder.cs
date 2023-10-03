using AutoFixture;
using Moq;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CreateCalendarEvent.CreateCalendarEventCommandValidatorTests;

public class CreateCalendarEventCommandValidatorBuilder
{
    public Mock<ICalendarsReadRepository> CalendarRepoMock { get; set; } = new();
    public Mock<IRegionsReadRepository> RegionRepoMock { get; set; } = new();

    public static CreateCalendarEventCommandValidator Create() => new CreateCalendarEventCommandValidatorBuilder().AddCalendarData().AddRegionData().Create();
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

    public static CreateCalendarEventCommandValidatorBuilder AddRegionData(this CreateCalendarEventCommandValidatorBuilder builder)
    {
        Fixture fixture = new();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var i = 1;
        var regions = fixture
            .Build<Region>()
            .With(c => c.Id, () => i++)
            .CreateMany()
            .ToList();
        CancellationToken cancellationToken = new();
        builder.RegionRepoMock.Setup(r => r.GetAllRegions(cancellationToken)).ReturnsAsync(regions);
        return builder;
    }

    public static CreateCalendarEventCommandValidator Create(this CreateCalendarEventCommandValidatorBuilder builder) =>
        new CreateCalendarEventCommandValidator(builder.CalendarRepoMock.Object, builder.RegionRepoMock.Object);

}
