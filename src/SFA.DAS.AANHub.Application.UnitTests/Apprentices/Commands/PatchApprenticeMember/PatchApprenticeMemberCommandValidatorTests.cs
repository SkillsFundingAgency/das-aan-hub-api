using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Apprentices.Commands.PatchApprenticeMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static NLog.LayoutRenderers.Wrappers.ReplaceLayoutRendererWrapper;

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
                Patchdoc = new JsonPatchDocument<Apprentice>()
            };

            command.Patchdoc = new JsonPatchDocument<Apprentice>
            {
                Operations =
                {
                    new Operation<Apprentice>
                        { op = operation, path = Name, value = nameValue }
                }
            };

            var member = new Member() { Status = Domain.Common.Constants.MembershipStatus.Live };
            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Patchdoc.Operations.Count(c => c.op.Equals(operation)));
            else
                result.ShouldHaveValidationErrorFor(p => p.Patchdoc.Operations.Count(c => c.op.Equals(operation)));
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
                Patchdoc = new JsonPatchDocument<Apprentice>()
            };

            var member = new Member() { Status = Domain.Common.Constants.MembershipStatus.Live };
            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Patchdoc.Operations.Count);
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual(PatchApprenticeMemberCommandValidator.NoPatchOperationsPresentErrorMessage, result.Errors[0].ErrorMessage);
        }

        [TestCase(Name, "nameValue1", "nameValue1", false)]
        [TestCase(Email, "email1@email.com", "email2@email.com", false)]
        public async Task ValidatePatchDoc_DuplicateReplaceOperation_InvalidResponse(string patchfield, string value1, string value2, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = 123;

            var command = new PatchApprenticeMemberCommand
            {
                RequestedByMemberId = memberId,
                ApprenticeId = apprenticeId,
                Patchdoc = new JsonPatchDocument<Apprentice>()
            };

            command.Patchdoc = new JsonPatchDocument<Apprentice>
            {
                Operations =
                {
                    new Operation<Apprentice>
                        { op = nameof(OperationType.Replace), path = patchfield, value = value1 },
                    new Operation<Apprentice>
                        { op = nameof(OperationType.Replace), path = patchfield, value = value2 }
                }
            };

            var member = new Member() { Status = Domain.Common.Constants.MembershipStatus.Live };
            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);
            var result = await sut.TestValidateAsync(command);

            result.ShouldHaveValidationErrorFor(c => c.Patchdoc.Operations.Count(operation => operation.path == Name && operation.op.Equals(nameof(OperationType.Replace), StringComparison.CurrentCultureIgnoreCase)));
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Count > 0);
            Assert.AreEqual(PatchApprenticeMemberCommandValidator.PatchOperationsLimitExceededErrorMessage, result.Errors[0].ErrorMessage);
        }
    }
}