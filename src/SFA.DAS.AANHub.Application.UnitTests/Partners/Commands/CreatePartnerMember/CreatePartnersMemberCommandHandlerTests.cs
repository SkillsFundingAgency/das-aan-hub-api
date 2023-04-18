using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.CreatePartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.CreatePartnerMember
{
    public class CreatePartnersMemberCommandHandlerTests
    {
        [Test]
        [AutoMoqData]
        public async Task Handle_AddsNewPartner(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreatePartnerMemberCommandHandler sut,
            CreatePartnerMemberCommand command)
        {
            var response = await sut.Handle(command, new CancellationToken());
            response.Result.MemberId.Should().Be(command.Id);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id && x.RegionId == command.RegionId)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.Id)));
        }

        [Test]
        [AutoMoqData]
        public async Task Handle_AddsNewPartner_WithNullRegions(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreatePartnerMemberCommandHandler sut,
            CreatePartnerMemberCommand command)
        {
            command.RegionId = null;

            var response = await sut.Handle(command, new CancellationToken());

            response.Result.MemberId.Should().Be(command.Id);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id && x.RegionId == command.RegionId)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.Id)));
        }
    }
}