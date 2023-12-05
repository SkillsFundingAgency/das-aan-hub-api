using AutoFixture;
using Moq;
using SFA.DAS.AANHub.Application.CalendarEvents.Commands.CreateCalendarEvent;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.CalendarEvents.Commands.CalendarEventCommandBaseValidatorTests;

public class CalendarEventCommandBaseValidatorBuilder
{
    public Mock<ICalendarsReadRepository> CalendarRepoMock { get; set; } = new();
    public Mock<IRegionsReadRepository> RegionRepoMock { get; set; } = new();
    public Mock<IMembersReadRepository> MembersRepoMock { get; set; } = new();

    public const string AdminActiveMemberId = "9278555b-5828-46b7-b7a1-8d07a4bbb6a5";
    public const string EmployerRegionalChairActiveMemberId = "7920698b-ed57-40e2-97de-aedcace22e66";
    public const string ApprenticeRegionalChairActiveMemberId = "d7f6a07b-93ed-4701-9b89-418ef5d67041";
    public const string EmployerActiveMemberId = "4c4522aa-ba6e-4770-ab7d-a0e518af24b2";
    public const string ApprenticeActiveMemberId = "ab26f712-ef28-4d77-890c-eff2a9b88485";
    public const string ApprenticeRegionalChairInactiveMemberId = "7eed98dd-04fb-4230-bf3a-0ca080608551";
    public const string EmployerRegionalChairInactiveMemberId = "2b8a2fc7-bc2b-4639-877f-8b6971341473";
    public const string AdminInactiveMemberId = "78c047a5-3018-444d-8ac0-e6ed0b92abc8";

    public static CalendarEventCommandBaseValidator Create() => new CalendarEventCommandBaseValidatorBuilder().AddCalendarData().AddRegionData().AddMembers().Create();
}

public static class CalendarEventCommandBaseValidatorBuilderExtensions
{
    public static Guid ToGuid(this string validGuid) => Guid.Parse(validGuid);
    public static CalendarEventCommandBaseValidatorBuilder AddCalendarData(this CalendarEventCommandBaseValidatorBuilder builder)
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

    public static CalendarEventCommandBaseValidatorBuilder AddRegionData(this CalendarEventCommandBaseValidatorBuilder builder)
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

    public static CalendarEventCommandBaseValidatorBuilder AddMembers(this CalendarEventCommandBaseValidatorBuilder builder)
    {
        Fixture fixture = new();
        fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        var adminMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Admin)
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, (bool?)null)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.AdminActiveMemberId.ToGuid())).ReturnsAsync(adminMember);

        var adminInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.AdminInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Admin)
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, (bool?)null)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.AdminInactiveMemberId.ToGuid())).ReturnsAsync(adminInactiveMember);

        var employerRegionalChairMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer)
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairActiveMemberId.ToGuid())).ReturnsAsync(employerRegionalChairMember);

        var employerRegionalChairInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer)
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.EmployerRegionalChairInactiveMemberId.ToGuid())).ReturnsAsync(employerRegionalChairInactiveMember);

        var apprenticeRegionalChairMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice)
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairActiveMemberId.ToGuid())).ReturnsAsync(apprenticeRegionalChairMember);

        var apprenticeRegionalChairInactiveMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairInactiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice)
            .With(m => m.Status, MembershipStatusType.Deleted.ToString())
            .With(m => m.IsRegionalChair, true)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.ApprenticeRegionalChairInactiveMemberId.ToGuid())).ReturnsAsync(apprenticeRegionalChairInactiveMember);

        var employerMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.EmployerActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Employer)
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, false)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.EmployerActiveMemberId.ToGuid())).ReturnsAsync(employerMember);

        var apprenticeMember = fixture
            .Build<Member>()
            .With(m => m.Id, CalendarEventCommandBaseValidatorBuilder.ApprenticeActiveMemberId.ToGuid())
            .With(m => m.UserType, UserType.Apprentice)
            .With(m => m.Status, MembershipStatusType.Live.ToString())
            .With(m => m.IsRegionalChair, false)
            .Create();
        builder.MembersRepoMock.Setup(r => r.GetMember(CalendarEventCommandBaseValidatorBuilder.ApprenticeActiveMemberId.ToGuid())).ReturnsAsync(apprenticeMember);

        return builder;
    }

    public static CalendarEventCommandBaseValidator Create(this CalendarEventCommandBaseValidatorBuilder builder) =>
        new CalendarEventCommandBaseValidator(builder.CalendarRepoMock.Object, builder.RegionRepoMock.Object, builder.MembersRepoMock.Object);

}
