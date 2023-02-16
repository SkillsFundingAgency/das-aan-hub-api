
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Text.Json;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.PatchPartnerMember
{
    public class PatchPartnerMemberCommandHandlerTests
    {
        private const string Email = "Email";
        private const string Name = "Name";
        private const string Organisation = "Organisation";

        [Test, AutoMoqData]
        public async Task Handle_DataFound_Patch(
            [Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IPartnersWriteRepository> partnersWriteRepoMock,
            [Frozen] Mock<IAuditWriteRepository> auditWriteRepository,
            PatchPartnerMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            Partner partner,
            CancellationToken cancellationToken)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            var emailValue = "email@w3c.com";
            var nameValue = "w3c";
            var organisationValue = "w3c.com";

            membersReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            partnersWriteRepoMock.Setup(r => r.GetPatchPartner(It.Is<string>(i => i == userName))).ReturnsAsync(partner);

            Func<string, Partner> getAudit = (auditBeforeOrAfter) => JsonSerializer.Deserialize<Partner>(auditBeforeOrAfter)!;


            var patchDoc = new JsonPatchDocument<Partner>
            {
                Operations =
                {
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue },
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue },
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Organisation, value = organisationValue }
                }
            };

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };
            dateMock.Setup(d => d.Now).Returns(expectedDate);
            var response = await sut.Handle(command, cancellationToken);

            Assert.NotNull(response);
            response.Result.IsSuccess.Should().BeTrue();
            response.IsValidResponse.Should().BeTrue();
            response.Errors.Count.Should().Be(0);

            Assert.AreEqual(emailValue, partner.Email);
            Assert.AreEqual(nameValue, partner.Name);
            Assert.AreEqual(organisationValue, partner.Organisation);
            
            Assert.NotNull(partner.LastUpdated);
            
            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x =>
                getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == partner.LastUpdated.GetValueOrDefault().Day)));;
        }

        [Test, AutoMoqData]
        public async Task Handle_NoDataFound(
            [Frozen] Mock<IPartnersWriteRepository> editRepoMock,
            PatchPartnerMemberCommandHandler sut,
            Partner partner,
            CancellationToken cancellationToken)
        {
            var userName = "username";
            var requestedByMemberId = Guid.NewGuid();
            editRepoMock.Setup(r => r.GetPatchPartner(It.Is<string>(i => i == userName))).ReturnsAsync((Partner?)null);

            var patchDoc = new JsonPatchDocument<Partner>();
            patchDoc.Replace(path => path.Email, partner.Email);
            patchDoc.Replace(path => path.Name, partner.Name);
            patchDoc.Replace(path => path.Organisation, partner.Organisation);

            var command = new PatchPartnerMemberCommand
            {
                UserName = userName,
                RequestedByMemberId = requestedByMemberId,
                PatchDoc = patchDoc
            };

            var response = await sut.Handle(command, cancellationToken);
            Assert.IsFalse(response.Result.IsSuccess);
        }

        [Test, AutoMoqData]
        public async Task Handle_OnSuccess_CreatesAudit([Frozen] Mock<IDateTimeProvider> dateMock,
            [Frozen] Mock<IMembersReadRepository> membersReadRepository,
            [Frozen] Mock<IPartnersWriteRepository> partnersWriteRepoMock,
            [Frozen]Mock<IAuditWriteRepository> auditWriteRepository,
            PatchPartnerMemberCommandHandler sut,
            DateTime expectedDate,
            Member member,
            CancellationToken cancellationToken)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            var emailValue = "email@w3c.com";
            Partner partner = new();

            membersReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            partnersWriteRepoMock.Setup(r => r.GetPatchPartner(It.Is<string>(i => i == userName))).ReturnsAsync(partner);

            var lastUpdatedBefore = partner.LastUpdated;

            var patchDoc = new JsonPatchDocument<Partner>
            {
                Operations =
                {
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = patchDoc
            };

            dateMock.Setup(d => d.Now).Returns(expectedDate);
            await sut.Handle(command, cancellationToken);

            Func<string, Partner> getAudit = (auditBeforeOrAfter) => JsonSerializer.Deserialize<Partner>(auditBeforeOrAfter)!;

            auditWriteRepository.Verify(a => a.Create(It.Is<Audit>(x => 
                x.Action == "Patch" 
                && x.ActionedBy == command.RequestedByMemberId 
                && x.AuditTime.Day == expectedDate.Day
                && getAudit(x!.Before!).LastUpdated.GetValueOrDefault().Day == lastUpdatedBefore.GetValueOrDefault().Day
                && getAudit(x!.After!).LastUpdated.GetValueOrDefault().Day == expectedDate.Day
                && x.Resource == MembershipUserType.Partner)));
        }
    }
}
