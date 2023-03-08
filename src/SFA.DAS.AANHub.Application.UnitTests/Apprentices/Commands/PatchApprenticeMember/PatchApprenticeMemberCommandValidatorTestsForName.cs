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
    public class PatchApprenticeMemberCommandValidatorTestsForName
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;
        public PatchApprenticeMemberCommandValidatorTestsForName() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Name = "Name";

        [TestCase("lorem epsum", true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Name_Empty(string nameValue, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var apprenticeId = Guid.NewGuid();

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
                        path = Name,
                        value = nameValue
                    }
                }
            };

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

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
            var apprenticeId = Guid.NewGuid();
            string nameValue = new('a', stringLength);

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
                        path = Name,
                        value = nameValue
                    }
                }
            };

            var sut = new PatchApprenticeMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Name);
            else
                result.ShouldHaveValidationErrorFor(p => p.Name);
        }
    }
}