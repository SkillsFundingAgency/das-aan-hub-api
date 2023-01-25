using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands.PatchApprenticeMember
{
    public class PatchApprenticeMemberCommandHandlerTests
    {

        [Test, AutoMoqData]
        public async Task Handle_DataFound_Patch(
            [Frozen] Mock<IApprenticesWriteRepository> editRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchApprenticeMemberCommandHandler sut,
            Apprentice apprentice,
            CancellationToken cancellationToken)
        {
            var apprenticeId = 123;
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetApprentice(It.Is<long>(i => i == apprenticeId))).ReturnsAsync(apprentice);

            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Replace(path => path.Email, apprentice.Email);
            patchDoc.Replace(path => path.Name, apprentice.Name);

            var command = new PatchApprenticeMemberCommand
            {
                ApprenticeId = apprenticeId,
                RequestedByMemberId = requestedByMemberId,
                Patchdoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            response.Errors.Count.Should().Be(0);

            auditWriteRepository.Verify(p => p.Create(It.Is<Audit>(x => x.Action == "Patch" && x.ActionedBy == command.RequestedByMemberId && x.Resource == MembershipUserType.Apprentice)));
        }

        [Test, AutoMoqData]
        public async Task Handle_NoDataFound(
            [Frozen] Mock<IApprenticesWriteRepository> editRepoMock,
            PatchApprenticeMemberCommandHandler sut,
            Apprentice apprentice,
            CancellationToken cancellationToken)
        {
            var apprenticeId = 123;
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetApprentice(It.Is<long>(i => i == apprenticeId))).ReturnsAsync((Apprentice?)null);

            var patchDoc = new JsonPatchDocument<Apprentice>();
            patchDoc.Replace(path => path.Email, apprentice.Email);
            patchDoc.Replace(path => path.Name, apprentice.Name);

            var command = new PatchApprenticeMemberCommand
            {
                ApprenticeId = apprenticeId,
                RequestedByMemberId = requestedByMemberId,
                Patchdoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.IsFalse(response.Result.IsSuccess);
        }
    }
}