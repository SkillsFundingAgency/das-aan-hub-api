using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Apprentices.Commands.PatchApprenticeMember
{
    [TestFixture]
    public class PatchApprenticeMemberCommandValidatorTests
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchApprenticeMemberCommandValidatorTests() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Name = "Name";
        private const string Email = "Email";

        [TestCase("replace", true)]
        [TestCase("remove", false)]
        public async Task ValidatePatchDoc_InvalidOperation_InvalidResponse(string operation, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;
            var nameValue = "test";

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                PatchDoc = new JsonPatchDocument<Apprentice>()
            };

            command.PatchDoc = new JsonPatchDocument<Apprentice>
            {
                Operations =
                {
                    new Operation<Apprentice>
                    {
                        op = operation,
                        path = Name,
                        value = nameValue
                    }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
            else
                result.ShouldHaveValidationErrorFor(p => p.PatchDoc.Operations.Count(c => c.op.Equals(operation)));
        }

        [Test]
        public async Task ValidatePatchDoc_NoOperations_InvalidResponse()
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                PatchDoc = new JsonPatchDocument<Apprentice>()
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are no patch operations in this request", result.Errors[0].ErrorMessage);
        }

        [TestCase(Name, "nameValue1", "nameValue1")]
        [TestCase(Email, "email1@email.com", "email2@email.com")]
        public async Task ValidatePatchDoc_DuplicateReplaceOperation_InvalidResponse(string patchField, string value1, string value2)
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                PatchDoc = new JsonPatchDocument<Apprentice>()
            };

            command.PatchDoc = new JsonPatchDocument<Apprentice>
            {
                Operations =
                {
                    new Operation<Apprentice>
                    {
                        op = nameof(OperationType.Replace),
                        path = patchField,
                        value = value1
                    },
                    new Operation<Apprentice>
                    {
                        op = nameof(OperationType.Replace),
                        path = patchField,
                        value = value2
                    }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.PatchDoc.Operations.Count(operation
                => operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)));

            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual("There are duplicate patch operations in this request", result.Errors[0].ErrorMessage);
        }
    }
}