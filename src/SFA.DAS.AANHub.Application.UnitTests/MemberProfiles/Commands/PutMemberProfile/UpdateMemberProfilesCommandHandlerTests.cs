using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.AANHub.Application.MemberProfiles.Commands.PutMemberProfile;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Commands.PutMemberProfile;
public class UpdateMemberProfilesCommandHandlerTests
{
    //[Test, MoqAutoData]
    public async Task Handle_AddsNewEmployer(
        [Frozen] Mock<IAanDataContext> aanDataContext,
        [Frozen] Mock<IMembersWriteRepository> membersWriteRepository,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        UpdateMemberProfilesCommandHandler sut,
        Member existingMember,
        UpdateMemberProfilesCommand command)
    {
        membersWriteRepository.Setup(x => x.Get(existingMember.Id)).ReturnsAsync(() => null);
        command.MemberId = existingMember.Id;
        command.RequestedByMemberId = existingMember.Id;

        await sut.Handle(command, new CancellationToken());

        membersWriteRepository.Verify(p => p.Create(It.IsAny<Member>()), Times.Never);
        auditWriteRepository.Verify(p => p.Create(It.IsAny<Audit>()), Times.Never);
        aanDataContext.Verify(a => a.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
