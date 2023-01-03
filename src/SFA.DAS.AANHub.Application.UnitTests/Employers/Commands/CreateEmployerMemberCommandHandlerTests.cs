using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Enums;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands
{
    public class CreateEmployerMemberCommandHandlerTests
    {
        [Test, AutoMoqData]
        public async Task Handle_AddsNewEmployer(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            [Frozen] Mock<IAanDataContext> aanContext,
            CreateEmployerMemberCommandHandler sut,
            CreateEmployerMemberCommand command)
        {

            var response = await sut.Handle(command, new CancellationToken());
            response.MemberId.Should().Be(command.Id);
            response.Status.Should().Be(MembershipStatus.Live.ToString());

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.RequestedByUserId)));
        }
    }
}
