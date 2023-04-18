using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands
{
    public class CreateApprenticeMemberCommandHandlerTests
    {
        [Test]
        [AutoMoqData]
        public async Task Handle_AddsNewApprentice(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreateApprenticeMemberCommandHandler sut,
            CreateApprenticeMemberCommand command)
        {
            var response = await sut.Handle(command, new CancellationToken());
            response.Result.MemberId.Should().Be(command.Id);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.Id)));
        }
    }
}