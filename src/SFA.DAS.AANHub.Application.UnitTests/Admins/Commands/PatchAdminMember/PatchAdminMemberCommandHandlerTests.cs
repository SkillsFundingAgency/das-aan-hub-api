using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Commands.PatchAdminMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Commands.PatchAdminMember
{
    public class PatchAdminMemberCommandHandlerTests
    {
        private const string Email = "Email";
        private const string Name = "Name";

        [Test]
        [AutoMoqData]
        public async Task Handle_DataFound_Patch(
            [Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IAdminsWriteRepository> adminsWriteRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchAdminMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            Admin admin,
            CancellationToken cancellationToken)
        {
            var requestedByMemberId = Guid.NewGuid();
            var userName = "username";
            var emailValue = "email@w3c.com";
            var nameValue = "w3c";

            membersReadRepository.Setup(a => a.GetMember(requestedByMemberId)).ReturnsAsync(member);
            adminsWriteRepoMock.Setup(r => r.GetPatchAdmin(It.Is<string>(i => i == userName))).ReturnsAsync(admin);

            Func<string, Admin> getAudit = auditBeforeOrAfter => JsonSerializer.Deserialize<Admin>(auditBeforeOrAfter)!;


            var patchDoc = new JsonPatchDocument<Admin>
            {
                Operations =
                {
                    new Operation<Admin>
                    {
                        op = nameof(OperationType.Replace),
                        path = Email,
                        value = emailValue
                    },
                    new Operation<Admin>
                    {
                        op = nameof(OperationType.Replace),
                        path = Name,
                        value = nameValue
                    }
                }
            };

            var command = new PatchAdminMemberCommand
            {
                RequestedByMemberId = requestedByMemberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            dateMock.Setup(d => d.Now).Returns(expectedDate);
            var response = await sut.Handle(command, cancellationToken);

            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            response.Errors.Count.Should().Be(0);

            Assert.AreEqual(emailValue, admin.Email);
            Assert.AreEqual(nameValue, admin.Name);

            Assert.NotNull(admin.LastUpdated);

            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x =>
                getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == admin.LastUpdated.GetValueOrDefault().Day)));
        }

        [Test]
        [AutoMoqData]
        public async Task Handle_NoDataFound(
            [Frozen] Mock<IAdminsWriteRepository> editRepoMock,
            PatchAdminMemberCommandHandler sut,
            Admin admin,
            CancellationToken cancellationToken)
        {
            var userName = "username";
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

        [Test]
        [AutoMoqData]
        public async Task Handle_OnSuccess_CreatesAudit([Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IAdminsWriteRepository> editRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchAdminMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            CancellationToken cancellationToken)
        {
            var requestedByMemberId = Guid.NewGuid();
            var userName = "username";
            var emailValue = "email@w3c.com";
            Admin admin = new();

            membersReadRepository.Setup(a => a.GetMember(requestedByMemberId)).ReturnsAsync(member);
            editRepoMock.Setup(r => r.GetPatchAdmin(It.Is<string>(i => i == userName))).ReturnsAsync(admin);

            var lastUpdatedBefore = admin.LastUpdated;

            var patchDoc = new JsonPatchDocument<Admin>
            {
                Operations =
                {
                    new Operation<Admin>
                    {
                        op = nameof(OperationType.Replace),
                        path = Email,
                        value = emailValue
                    }
                }
            };

            var command = new PatchAdminMemberCommand
            {
                RequestedByMemberId = requestedByMemberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            dateMock.Setup(d => d.Now).Returns(expectedDate);
            await sut.Handle(command, cancellationToken);

            Func<string, Admin> getAudit = auditBeforeOrAfter => JsonSerializer.Deserialize<Admin>(auditBeforeOrAfter)!;

            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x =>
                x.Action == "Patch"
                && x.ActionedBy == command.RequestedByMemberId
                && x.AuditTime.Day == expectedDate.Day
                && getAudit(x!.Before!).LastUpdated.GetValueOrDefault().Day == lastUpdatedBefore.GetValueOrDefault().Day
                && getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == expectedDate.Day
                && x.Resource == MembershipUserType.Admin)));
        }
    }
}