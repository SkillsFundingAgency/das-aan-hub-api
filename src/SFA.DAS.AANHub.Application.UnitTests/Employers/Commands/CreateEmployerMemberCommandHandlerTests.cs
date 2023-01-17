using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands
{
    public class CreateEmployerMemberCommandHandlerTests
    {
        [Test, AutoMoqData]
        public async Task Handle_AddsNewEmployer(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreateEmployerMemberCommandHandler sut,
            CreateEmployerMemberCommand command)
        {
            command.Regions = new List<int>(new[] { 1 });
            var response = await sut.Handle(command, new CancellationToken());
            response.MemberId.Should().Be(command.Id);
            response.Status.Should().Be(MembershipStatus.Live.ToString());

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id)));
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.MemberRegions != null && x.MemberRegions[0].RegionId == 1)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.RequestedByMemberId)));
        }

        [Test, AutoMoqData]
        public async Task Handle_AddsNewEmployer_WithNullRegions(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreateEmployerMemberCommandHandler sut,
            CreateEmployerMemberCommand command)
        {
            command.Regions = null;

            var response = await sut.Handle(command, new CancellationToken());
            response.MemberId.Should().Be(command.Id);
            response.Status.Should().Be(MembershipStatus.Live);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.RequestedByMemberId)));
        }
    }
}
