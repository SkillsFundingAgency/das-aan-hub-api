using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Commands.PatchPartnerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Commands.PatchPartnerMember
{
    [TestFixture]
    public class PatchPartnerMemberCommandValidatorTestsForEmail
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchPartnerMemberCommandValidatorTestsForEmail() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Email = "Email";

        [TestCase("john.doe@example.com", true)]
        [TestCase("@example.com", false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Email_Empty_Format(string emailValue, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Partner>()
            };

            command.PatchDoc = new JsonPatchDocument<Partner>
            {
                Operations =
                {
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchPartnerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Email);
            else
                result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [TestCase(10, "@email.com", true)]
        [TestCase(257, "@email.com", false)]
        [TestCase(0, "", false)]
        public async Task Validate_Patch_Email_Length(int stringLength, string emailSuffix, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            string emailValue = new string('a', stringLength) + emailSuffix;

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Partner>()
            };

            command.PatchDoc = new JsonPatchDocument<Partner>
            {
                Operations =
                {
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);
            
            var sut = new PatchPartnerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Email);
            else
                result.ShouldHaveValidationErrorFor(p => p.Email);
        }
    }
}