using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Application.Services;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.AANHub.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands;

public class CreateApprenticeMemberCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AddsNewApprentice(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<INotificationsWriteRepository> notificationsWriteRepository,
        [Frozen] Mock<IRegionsReadRepository> regionsReadRepository,
        Region region,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command)
    {
        regionsReadRepository.Setup(x => x.GetRegionById(It.IsAny<int>(), CancellationToken.None)).ReturnsAsync(region);
        var response = await sut.Handle(command, new CancellationToken());

        var mockRegion = await regionsReadRepository.Object.GetRegionById(command.RegionId.GetValueOrDefault(), CancellationToken.None);
        var mockToken = new OnboardingEmailTemplate(command.FirstName!, command.LastName!, $"{mockRegion!.Area} team");
        var mockTokenSerialised = JsonSerializer.Serialize(mockToken);

        using (new AssertionScope())
        {
            response.Result.MemberId.Should().Be(command.MemberId);
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.MemberId == command.MemberId)));
            notificationsWriteRepository.Verify(p => p.Create(It.Is<Notification>(x => x.Tokens == mockTokenSerialised)));
            regionsReadRepository.Verify(p => p.GetRegionById(It.Is<int>(x => x == command.RegionId), CancellationToken.None));
        }
    }

    [Test, MoqAutoData]
    public async Task Handle_AddsNewApprentice_WithDefaultMemberPreference(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());

        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId && x.MemberPreferences.Count == MemberPreferenceService.GetDefaultMemberPreferences(UserType.Apprentice).Count)));
    }
}