using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Common;
using SFA.DAS.AANHub.Application.Employers.Commands.CreateEmployerMember;
using SFA.DAS.AANHub.Application.Services;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.CreateEmployerMember;

public class CreateEmployerMemberCommandHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_AddsNewEmployer(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IRegionsReadRepository> regionsReadRepository,
        [Frozen] Mock<IMemberPreferenceWriteRepository> memberPreferenceWriteRepository,
        Region region,
        CreateEmployerMemberCommandHandler sut,
        CreateEmployerMemberCommand command)
    {
        region.Id = (int)command.RegionId!;
        regionsReadRepository.Setup(x => x.GetRegionById(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(region);

        var response = await sut.Handle(command, new CancellationToken());
        var mockRegion = await regionsReadRepository.Object.GetRegionById(command.RegionId.GetValueOrDefault(), CancellationToken.None);
        var mockToken = new OnboardingEmailTemplate(command.FirstName!, command.LastName!, $"{mockRegion!.Area} team");
        var mockTokenSerialised = JsonSerializer.Serialize(mockToken);

        using (new AssertionScope())
        {
            response.Result.MemberId.Should().Be(command.MemberId);
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x =>
                x.Id == command.MemberId
                && x.RegionId == command.RegionId
            )));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.MemberId == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.Tokens == mockTokenSerialised)));
            regionsReadRepository.Verify(p => p.GetRegionById(It.Is<int>(x => x == command.RegionId), CancellationToken.None));
            memberPreferenceWriteRepository.Verify(p => p.Create(It.Is<MemberPreference>(x => x.MemberId == command.MemberId)), Times.Exactly(MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count));
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_AddsNewEmployer_WithNullRegions(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IRegionsReadRepository> regionsReadRepository,
        [Frozen] Mock<IMemberPreferenceWriteRepository> memberPreferenceWriteRepository,
        Region region,
        CreateEmployerMemberCommandHandler sut,
        CreateEmployerMemberCommand command)
    {
        command.RegionId = null;
        region.Area = "Multi-region team";
        regionsReadRepository.Setup(x => x.GetRegionById(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(region);

        var response = await sut.Handle(command, new CancellationToken());
        var mockToken = new OnboardingEmailTemplate(command.FirstName!, command.LastName!, $"{region!.Area} team");
        var mockTokenSerialised = JsonSerializer.Serialize(mockToken);

        using (new AssertionScope())
        {
            response.Result.MemberId.Should().Be(command.MemberId);
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId && x.RegionId == null)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.MemberId == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.Tokens == mockTokenSerialised)));
            regionsReadRepository.Verify((p => p.GetRegionById(It.Is<int>(x => x == command.RegionId), CancellationToken.None)), Times.Never);
            memberPreferenceWriteRepository.Verify(p => p.Create(It.Is<MemberPreference>(x => x.MemberId == command.MemberId)), Times.Exactly(MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count));
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_AddsNewEmployer_WithDefaultMemberPreference(
        CreateEmployerMemberCommand command,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IMemberPreferenceWriteRepository> memberPreferenceWriteRepository,
        [Greedy] CreateEmployerMemberCommandHandler sut)
    {
        var response = await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            response.Result.MemberId.Should().Be(command.MemberId);
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId && x.MemberPreferences.Count == MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count)));
            memberPreferenceWriteRepository.Verify(p => p.Create(It.Is<MemberPreference>(x => x.MemberId == command.MemberId)), Times.Exactly(MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count));
        }
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_AddsNewEmployer_WithDefaultMemberPreferenceAndNotificationProperties(
    CreateEmployerMemberCommand command,
    [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
    [Frozen] Mock<IMemberPreferenceWriteRepository> memberPreferenceWriteRepository,
    [Greedy] CreateEmployerMemberCommandHandler sut)
    {
        command.MemberNotificationEventFormatValues = new List<MemberNotificationEventFormatValues>
    {
        new MemberNotificationEventFormatValues { EventFormat = "InPerson", Ordering = 1, ReceiveNotifications = true },
        new MemberNotificationEventFormatValues { EventFormat = "Online", Ordering = 2, ReceiveNotifications = false }
    };

        command.MemberNotificationLocationValues = new List<MemberNotificationLocationValues>
    {
        new MemberNotificationLocationValues { Name = "Location 1", Radius = 5, Latitude = 51.5074, Longitude = -0.1278 },
        new MemberNotificationLocationValues { Name = "Location 2", Radius = 10, Latitude = 40.7128, Longitude = -74.0060 }
    };

        var response = await sut.Handle(command, new CancellationToken());

        using (new AssertionScope())
        {
            response.Result.MemberId.Should().Be(command.MemberId);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x =>
                    x.Id == command.MemberId &&
                    x.MemberPreferences.Count == MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count &&
                    x.MemberNotificationEventFormats.Count == 2 &&
                    x.MemberNotificationEventFormats.Any(e =>
                        e.EventFormat == "InPerson" && e.ReceiveNotifications) &&
                    x.MemberNotificationEventFormats.Any(e =>
                        e.EventFormat == "Online" && !e.ReceiveNotifications) &&
                    x.MemberNotificationLocations.Count == 2 &&
                    x.MemberNotificationLocations.Any(l =>
                        l.Name == "Location 1" && l.Radius == 5 && l.Latitude == 51.5074 && l.Longitude == -0.1278) &&
                    x.MemberNotificationLocations.Any(l =>
                        l.Name == "Location 2" && l.Radius == 10 && l.Latitude == 40.7128 && l.Longitude == -74.0060)
                )), Times.Once);

            memberPreferenceWriteRepository.Verify(p => p.Create(It.Is<MemberPreference>(x => x.MemberId == command.MemberId)),
                    Times.Exactly(MemberPreferenceService.GetDefaultPreferencesForMember(UserType.Apprentice, Guid.NewGuid()).Count));
        }
    }
}
