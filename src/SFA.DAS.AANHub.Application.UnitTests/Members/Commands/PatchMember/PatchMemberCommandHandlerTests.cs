using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember;

public class PatchMemberCommandHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_DataFound_Patch(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
        [Frozen] Mock<IAanDataContext> dataContextMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        Member member,
        CancellationToken cancellationToken)
    {
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(member);

        var patchDoc = new JsonPatchDocument<Member>();
        patchDoc.Replace(path => path.Email, member.Email);
        command.PatchDoc = patchDoc;

        var response = await sut.Handle(command, cancellationToken);

        Assert.NotNull(response);
        response.Result.IsSuccess.Should().BeTrue();
        response.IsValidResponse.Should().BeTrue();

        auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "PatchMember" && x.ActionedBy == command.MemberId && x.Resource == "Member")));

        dataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_NoDataFound(
        [Frozen] Mock<IMembersWriteRepository> memberWriteRepoMock,
        PatchMemberCommandHandler sut,
        PatchMemberCommand command,
        CancellationToken cancellationToken)
    {
        memberWriteRepoMock.Setup(r => r.Get(command.MemberId)).ReturnsAsync(() => null);

        var response = await sut.Handle(command, cancellationToken);

        Assert.IsFalse(response.Result.IsSuccess);
    }
}
