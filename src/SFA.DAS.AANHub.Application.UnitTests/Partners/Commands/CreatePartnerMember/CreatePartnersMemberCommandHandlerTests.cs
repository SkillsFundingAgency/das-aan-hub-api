using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember;

public class CreatePartnersMemberCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_AddsNewPartner(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        CreatePartnerMemberCommandHandler sut,
        CreatePartnerMemberCommand command)
    {
        var response = await sut.Handle(command, new CancellationToken());
        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId && x.RegionId == command.RegionId)));
        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_AddsNewPartner_WithNullRegions(
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        CreatePartnerMemberCommandHandler sut,
        CreatePartnerMemberCommand command)
    {
        command.RegionId = null;

        var response = await sut.Handle(command, new CancellationToken());

        response.Result.MemberId.Should().Be(command.MemberId);

        membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.MemberId && x.RegionId == command.RegionId)));
        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.MemberId)));
    }
}