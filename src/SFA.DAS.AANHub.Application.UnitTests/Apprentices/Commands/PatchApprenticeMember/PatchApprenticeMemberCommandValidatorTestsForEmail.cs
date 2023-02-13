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
    public class PatchApprenticeMemberCommandValidatorTestsForEmail
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchApprenticeMemberCommandValidatorTestsForEmail() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Email = "Email";

        [TestCase("john.doe@example.com", true)]
        [TestCase("@example.com", false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Email_Format(string emailValue, bool isValid)
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
                        path = Email,
                        value = emailValue
                    }
                }
            };

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Email);
            else
                result.ShouldHaveValidationErrorFor(p => p.Email);
        }

        [TestCase("john.doe@example.com", true)]
        [TestCase("@example.com", false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Email_Empty_Not_Validated_Format(string emailValue, bool isValid)
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
                        path = Email,
                        value = emailValue
                    }
                }
            };

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

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
            var apprenticeId = 123;
            var emailValue = new string('a', stringLength) + emailSuffix;

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
                        path = Email,
                        value = emailValue
                    }
                }
            };

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Email);
            else
                result.ShouldHaveValidationErrorFor(p => p.Email);
        }
    }
}