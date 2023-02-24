﻿using FluentValidation.TestHelper;
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
    public class PatchPartnerMemberCommandValidatorTestsForName
    {
        private readonly Mock<IMembersReadRepository> _memberReadRepository;

        public PatchPartnerMemberCommandValidatorTestsForName() => _memberReadRepository = new Mock<IMembersReadRepository>();

        private const string Name = "Name";

        [TestCase("lorem epsum", true)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        public async Task Validate_Patch_Name_Empty(string nameValue, bool isValid)
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
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue }
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
                result.ShouldNotHaveValidationErrorFor(p => p.Name);
            else
                result.ShouldHaveValidationErrorFor(p => p.Name);
        }

        [TestCase(123, true)]
        [TestCase(251, false)]
        public async Task Validate_Patch_Name_Length(int stringLength, bool isValid)
        {
            var memberId = Guid.NewGuid();
            var userName = "username";
            string nameValue = new('a', stringLength);

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
                        { op = nameof(OperationType.Replace), path = Name, value = nameValue }
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
                result.ShouldNotHaveValidationErrorFor(p => p.Name);
            else
                result.ShouldHaveValidationErrorFor(p => p.Name);
        }
    }
}