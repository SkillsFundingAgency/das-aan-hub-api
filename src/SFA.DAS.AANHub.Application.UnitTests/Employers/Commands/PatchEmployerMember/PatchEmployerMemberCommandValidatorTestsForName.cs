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
    public class PatchEmployerMemberCommandValidatorTestsForName
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchEmployerMemberCommandValidatorTestsForName() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Name = "Name";

        [TestCase("lorem epsum", true)]
        [TestCase("", false)]
        public async Task Validate_Patch_Name_Empty(string nameValue, bool isValid)
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
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue }
                }
            };

            var sut = new PatchEmployerMemberCommandValidator
                (_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Name);
            else
                result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [TestCase(123, true)]
        [TestCase(251, false)]
        public async Task Validate_Patch_Name_Length(int stringLength, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();
            string nameValue = new('a', stringLength);

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
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue }
                }
            };

            var sut = new PatchEmployerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Name);
            else
                result.ShouldHaveValidationErrorFor(p => p.Name);
        }
    }
}