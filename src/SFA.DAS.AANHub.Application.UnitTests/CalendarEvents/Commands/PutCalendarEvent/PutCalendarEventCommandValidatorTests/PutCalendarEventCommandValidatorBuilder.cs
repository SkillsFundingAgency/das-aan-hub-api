using AutoFixture;
using Moq;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.PutCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
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
    public const string EmployerRegionalChairActiveMemberId = "7920698b-ed57-40e2-97de-aedcace22e66";
    public const string ApprenticeRegionalChairActiveMemberId = "d7f6a07b-93ed-4701-9b89-418ef5d67041";
    public const string EmployerActiveMemberId = "4c4522aa-ba6e-4770-ab7d-a0e518af24b2";
    public const string ApprenticeActiveMemberId = "ab26f712-ef28-4d77-890c-eff2a9b88485";
    public const string ApprenticeRegionalChairInactiveMemberId = "7eed98dd-04fb-4230-bf3a-0ca080608551";
    public const string EmployerRegionalChairInactiveMemberId = "2b8a2fc7-bc2b-4639-877f-8b6971341473";
    public const string AdminInactiveMemberId = "78c047a5-3018-444d-8ac0-e6ed0b92abc8";

    public const string EventNotFoundId = "c7be8d81-b811-43f2-bd92-e48a2724d7d3";
    public const string EventIsNotActiveId = "d19b813b-b5f2-40f1-b9f7-fdaf488283b7";
    public const string EventIsInPastId = "b09c88f7-01b6-48fe-a755-e9591d0da729";

    public static PutCalendarEventCommandValidator Create() => new PutCalendarEventCommandValidatorBuilder().AddCalendarData().AddRegionData().AddMembers().AddEvents().Create();
}

public static class PutCalendarEventCommandValidatorBuilderExtensions
{
    public static Guid ToGuid(this string validGuid) => Guid.Parse(validGuid);
    public static PutCalendarEventCommandValidatorBuilder AddCalendarData(this PutCalendarEventCommandValidatorBuilder builder)
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

    public static PutCalendarEventCommandValidatorBuilder AddRegionData(this PutCalendarEventCommandValidatorBuilder builder)
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

    public static PutCalendarEventCommandValidatorBuilder AddMembers(this PutCalendarEventCommandValidatorBuilder builder)
    {
        Fixture fixture = new();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var adminMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Admin.ToString())
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, (bool?)null)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.AdminActiveMemberId.ToGuid())).ReturnsAsync(adminMember);

        var adminInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.AdminInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Admin.ToString())
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, (bool?)null)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.AdminInactiveMemberId.ToGuid())).ReturnsAsync(adminInactiveMember);

        var employerRegionalChairMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer.ToString())
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairActiveMemberId.ToGuid())).ReturnsAsync(employerRegionalChairMember);

        var employerRegionalChairInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer.ToString())
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.EmployerRegionalChairInactiveMemberId.ToGuid())).ReturnsAsync(employerRegionalChairInactiveMember);

        var apprenticeRegionalChairMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice.ToString())
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairActiveMemberId.ToGuid())).ReturnsAsync(apprenticeRegionalChairMember);

        var apprenticeRegionalChairInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice.ToString())
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.ApprenticeRegionalChairInactiveMemberId.ToGuid())).ReturnsAsync(apprenticeRegionalChairInactiveMember);

        var employerMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.EmployerActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer.ToString())
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, false)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.EmployerActiveMemberId.ToGuid())).ReturnsAsync(employerMember);

        var apprenticeMember = fixture
            .Build<Member>()
            .With(m => m.Id, PutCalendarEventCommandValidatorBuilder.ApprenticeActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice.ToString())
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, false)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(PutCalendarEventCommandValidatorBuilder.ApprenticeActiveMemberId.ToGuid())).ReturnsAsync(apprenticeMember);

        return builder;
    }

    public static PutCalendarEventCommandValidatorBuilder AddEvents(this PutCalendarEventCommandValidatorBuilder builder)
    {
        Fixture fixture = new();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());

        builder.CalendarEventsRepoMock.Setup(r => r.GetCalendarEvent(PutCalendarEventCommandValidatorBuilder.EventNotFoundId.ToGuid())).ReturnsAsync((CalendarEvent)null);

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
