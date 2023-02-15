using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.PatchAdminMember
{
    public class PatchAdminMemberCommandHandlerTests
    {
        [Test]
        [AutoMoqData]
        public async Task Handle_DataFound_Patch(
            [Frozen] Mock<IAdminsWriteRepository> editRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchAdminMemberCommandHandler sut,
            Admin admin,
            CancellationToken cancellationToken)
        {
            var userName = "userName123";
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetPatchAdmin(It.Is<string>(i => i == userName))).ReturnsAsync(admin);

            var patchDoc = new JsonPatchDocument<Admin>();
            patchDoc.Replace(path => path.Email, admin.Email);
            patchDoc.Replace(path => path.Name, admin.Name);

            var command = new PatchAdminMemberCommand
            {
                UserName = userName,
                RequestedByMemberId = requestedByMemberId,
                PatchDoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            response.Errors.Count.Should().Be(0);

            auditWriteRepository.Verify(p
                => p.Create(It.Is<Audit>(x => x.Action == "Patch" && x.ActionedBy == command.RequestedByMemberId && x.Resource == MembershipUserType.Admin)));
        }

        [Test]
        [AutoMoqData]
        public async Task Handle_NoDataFound(
            [Frozen] Mock<IAdminsWriteRepository> editRepoMock,
            PatchAdminMemberCommandHandler sut,
            Admin admin,
            CancellationToken cancellationToken)
        {
            var userName = "userName123";
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetPatchAdmin(It.Is<string>(i => i == userName))).ReturnsAsync((Admin?)null);

            var patchDoc = new JsonPatchDocument<Admin>();
            patchDoc.Replace(path => path.Email, admin.Email);
            patchDoc.Replace(path => path.Name, admin.Name);

            var command = new PatchAdminMemberCommand
            {
                UserName = userName,
                RequestedByMemberId = requestedByMemberId,
                PatchDoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.IsFalse(response.Result.IsSuccess);
        }
    }
}