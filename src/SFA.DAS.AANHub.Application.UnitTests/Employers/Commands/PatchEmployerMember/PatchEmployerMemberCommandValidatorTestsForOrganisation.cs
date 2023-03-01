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
    public class PatchEmployerMemberCommandValidatorTestsForOrganisation
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchEmployerMemberCommandValidatorTestsForOrganisation() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Organisation = "Organisation";

        [TestCase("w3c", true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Organisation_Empty(string organisationeValue, bool isValid)
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
                        { op = nameof(OperationType.Replace), path = Organisation, value = organisationeValue }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchEmployerMemberCommandValidator
                (_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Organisation);
            else
                result.ShouldHaveValidationErrorFor(p => p.Organisation);
        }

        [TestCase(123, true)]
        [TestCase(201, false)]
        public async Task Validate_Patch_Organisation_Length(int stringLength, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userRef = Guid.NewGuid();
            string organisationeValue = new('a', stringLength);

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
                        { op = nameof(OperationType.Replace), path = Organisation, value = organisationeValue }
                }
            };

            var member = new Member
            {
                Status = Domain.Common.Constants.MembershipStatus.Live
            };

            _memberReadRepository.Setup(a => a.GetMember(memberId)).ReturnsAsync(member);

            var sut = new PatchEmployerMemberCommandValidator(_memberReadRepository.Object);

            var result = await sut.TestValidateAsync(command);

            if (isValid)
                result.ShouldNotHaveValidationErrorFor(p => p.Organisation);
            else
                result.ShouldHaveValidationErrorFor(p => p.Organisation);
        }
    }
}