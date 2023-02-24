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
    public class PatchPartnerMemberCommandValidatorTests
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchPartnerMemberCommandValidatorTests() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Name = "Name";
        private const string Email = "Email";
        private const string Organisation = "Organisation";

        [TestCase("replace", true)]
        [TestCase("remove", false)]
        public async Task Validate_Patch_Organisation_NoMatchingOperation_ErrorMessage(string operation, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            var nameValue = "test";

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
                        { op = operation, path = Organisation, value = nameValue }
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
                result.ShouldNotHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
            else
                result.ShouldHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
        }

        [Test]
        public async Task Validate_Patch_NoOperations_ErrorMessage()
        {
            var memberId = Guid.NewGuid();
            var userName = "username";

            var command = new PatchPartnerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserName = userName,
                PatchDoc = new JsonPatchDocument<Partner>()
            };

            var member = new Member() { Status = Domain.Common.Constants.MembershipStatus.Live };
            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchPartnerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are no patch operations in this request", result.Errors[0].ErrorMessage);
        }


        [TestCase(Name, "nameValue1", "nameValue1", false)]
        [TestCase(Email, "email1@email.com", "email2@email.com", false)]
        [TestCase(Organisation, "Organisation1", "Organisation2", false)]
        public async Task ValidatePatchDoc_DuplicateReplaceOperation_InvalidResponse(string patchfield, string value1, string value2, bool isValid)
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
                        { op = nameof(OperationType.Replace), path = patchfield, value = value1 },
                    new Operation<Partner>
                        { op = nameof(OperationType.Replace), path = patchfield, value = value2 }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchPartnerMemberCommandValidator(_memberReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count(operation => operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)));
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are duplicate patch operations in this request", result.Errors[0].ErrorMessage);
        }
    }
}