using FluentValidation.TestHelper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Employers.Commands.PatchEmployerMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Employers.Commands.PatchEmployerMember
{
    [TestFixture]
    public class PatchEmployerMemberCommandValidatorTestsForEmail
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchEmployerMemberCommandValidatorTestsForEmail() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Email = "Email";

        [TestCase("john.doe@example.com", true)]
        [TestCase("@example.com", false)]
        [TestCase("", false)]
        public async Task Validate_Patch_Email_Empty_Format(string emailValue, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = new JsonPatchDocument<Employer>()
            };

            command.PatchDoc = new JsonPatchDocument<Employer>
            {
                Operations =
                {
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var sut = new PatchEmployerMemberCommandValidator(_memberReadRepository.Object);

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
            var userRef = Guid.NewGuid();
            string emailValue = new string('a', stringLength) + emailSuffix;

            var command = new PatchEmployerMemberCommand
            {
                RequestedByMemberId = memberId,
                UserRef = userRef,
                PatchDoc = new JsonPatchDocument<Employer>()
            };

            command.PatchDoc = new JsonPatchDocument<Employer>
            {
                Operations =
                {
                    new Operation<Employer>
                        { op = nameof(OperationType.Replace), path = Email, value = emailValue }
                }
            };

            var sut = new PatchEmployerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Email);
            else
                result.ShouldHaveValidationErrorFor(p => p.Email);
        }
    }
}