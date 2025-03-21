﻿using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberLeaving;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PostMemberLeaving;
public class PostMemberLeavingCommandValidatorTests
{
    [TestCase(null, UserType.Apprentice, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Apprentice, false)]
    [TestCase("ac44a17b-f843-4e1f-979b-aa95c0fe44f2", UserType.Apprentice, false, MemberIdValidator.MemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Admin, true)]
    public async Task ValidateMemberId(string memberId, UserType userType, bool isValid, string? errorMessage = null)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        repositoryMock.Setup(r => r.GetMember(Guid.Parse("f5521677-7733-4416-b5a7-4c7a231fe469"))).ReturnsAsync(new Member() { Status = Domain.Common.Constants.MembershipStatus.Live, UserType = userType });
        PostMemberLeavingCommandValidator sut = new(repositoryMock.Object);
        PostMemberLeavingCommand target = new();
        if (memberId != null) target.MemberId = Guid.Parse(memberId);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.MemberId);
        }
        else
        {
            if (userType == UserType.Admin)
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? MemberIdValidator.MemberIdEmptyErrorMessage
                    : errorMessage);
            }
            else
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? MemberIdValidator.MemberIdEmptyErrorMessage
                    : errorMessage);
            }
        }
    }
}
