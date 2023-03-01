using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.PatchEmployerMember
{
    public class PatchEmployerMemberCommandHandlerTests
    {
        private const string Email = "Email";
        private const string Name = "Name";
        private const string Organisation = "Organisation";

        [Test, AutoMoqData]
        public async Task Handle_DataFound_Patch(
            [Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IEmployersWriteRepository> employersWriteRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchEmployerMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            Employer employer,
            CancellationToken cancellationToken)
        {
            var requestedByMemberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();
            const string emailValue = "email@w3c.com";
            const string nameValue = "w3c";
            const string organisationValue = "w3c.com";

            membersReadRepository.Setup(a => a.GetMember(requestedByMemberId)).ReturnsAsync(member);
            employersWriteRepoMock.Setup(r => r.GetPatchEmployer(It.Is<Guid>(i => i == userRef))).ReturnsAsync(employer);

            Func<string, Employer> getAudit = (auditBeforeOrAfter) => JsonSerializer.Deserialize<Employer>(auditBeforeOrAfter)!;

            var patchDoc = new JsonPatchDocument<Employer>
            {
                Operations =
                {
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue },
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue },
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Organisation, value = organisationValue }
                }
            };

            var command = new PatchEmployerMemberCommand
            {
                UserRef = userRef,
                RequestedByMemberId = requestedByMemberId,
                PatchDoc = patchDoc
            };

            dateMock.Setup(d => d.Now).Returns(expectedDate);
            var response = await sut.Handle(command, cancellationToken);

            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            response.Errors.Count.Should().Be(0);

            Assert.AreEqual(emailValue, employer.Email);
            Assert.AreEqual(nameValue, employer.Name);
            Assert.AreEqual(organisationValue, employer.Organisation);

            Assert.NotNull(employer.LastUpdated);

            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x =>
                getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == employer.LastUpdated.GetValueOrDefault().Day)));
        }

        [Test, AutoMoqData]
        public async Task Handle_NoDataFound(
            [Frozen] Mock<IEmployersWriteRepository> editRepoMock,
            PatchEmployerMemberCommandHandler sut,
            Employer employer,
            CancellationToken cancellationToken)
        {
            var userRef = Guid.NewGuid();
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetPatchEmployer(It.Is<Guid>(i => i == userRef))).ReturnsAsync((Employer?)null);

            var patchDoc = new JsonPatchDocument<Employer>();
            patchDoc.Replace(path => path.Email, employer.Email);
            patchDoc.Replace(path => path.Name, employer.Name);

            var command = new PatchEmployerMemberCommand
            {
                UserRef = userRef,
                RequestedByMemberId = requestedByMemberId,
                PatchDoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.IsFalse(response.Result.IsSuccess);
        }

        [Test, AutoMoqData]
        public async Task Handle_OnSuccess_CreatesAudit([Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IEmployersWriteRepository> employersWriteRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchEmployerMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            CancellationToken cancellationToken)
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();
            const string emailValue = "email@w3c.com";
            Employer employer = new();

            membersReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            employersWriteRepoMock.Setup(r => r.GetPatchEmployer(It.Is<Guid>(i => i == userRef))).ReturnsAsync(employer);

            var lastUpdatedBefore = employer.LastUpdated;

            var patchDoc = new JsonPatchDocument<Employer>
            {
                Operations =
                {
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = patchDoc
            };

            dateMock.Setup(d => d.Now).Returns(expectedDate);
            await sut.Handle(command, cancellationToken);

            Func<string, Employer> getAudit = (auditBeforeOrAfter) => JsonSerializer.Deserialize<Employer>(auditBeforeOrAfter)!;

            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x =>
                x.Action == "Patch"
                && x.ActionedBy == command.RequestedByMemberId
                && x.AuditTime.Day == expectedDate.Day
                && getAudit(x!.Before!).LastUpdated.GetValueOrDefault().Day == lastUpdatedBefore.GetValueOrDefault().Day
                && getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == expectedDate.Day
                && x.Resource == MembershipUserType.Employer)));
        }
    }
}
