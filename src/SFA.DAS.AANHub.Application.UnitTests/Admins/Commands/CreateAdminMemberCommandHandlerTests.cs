using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands
{
    public class CreateAdminMemberCommandHandlerTests
    {
        [Test, AutoMoqData]
        public async Task Handle_AddsNewAdmin(
            [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            CreateAdminMemberCommandHandler sut,
            CreateAdminMemberCommand command)
        {

            var response = await sut.Handle(command, new CancellationToken());
            response.Result.MemberId.Should().Be(command.Id);
            response.Result.Status.Should().Be(MembershipStatus.Live);

            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.Id == command.Id)));
            membersWriteRepository.Verify(p => p.Create(It.Is<Member>(x => x.UserType == MembershipUserType.Admin)));
            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.ActionedBy == command.RequestedByMemberId)));
        }
    }
}
