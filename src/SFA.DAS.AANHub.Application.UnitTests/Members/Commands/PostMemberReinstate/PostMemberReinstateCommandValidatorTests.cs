using FluentAssertions;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberReinstate;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PostMemberReinstate;
public class PostMemberReinstateCommandValidatorTests
{
    [TestCase(null, UserType.Apprentice, false)]
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Apprentice, false)]
    [TestCase("ac44a17b-f843-4e1f-979b-aa95c0fe44f2", UserType.Apprentice, false, PostMemberReinstateCommandValidator.MemberIdMustBeApprenticeOrEmployer)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Admin, false, PostMemberReinstateCommandValidator.MemberIdMustBeApprenticeOrEmployer)]
    public async Task ValidateMemberId(string memberId, UserType userType, bool isValid, string? errorMessage = null)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        repositoryMock.Setup(r => r.GetMember(Guid.Parse("f5521677-7733-4416-b5a7-4c7a231fe469"))).ReturnsAsync(new Member() { Status = Domain.Common.Constants.MembershipStatus.Withdrawn, UserType = userType });
        PostMemberReinstateCommandValidator sut = new(repositoryMock.Object);
        PostMemberReinstateCommand target = new();
        if (memberId != null) target.MemberId = Guid.Parse(memberId);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            // result.ShouldNotHaveValidationErrorFor(s => s.MemberId);
            result.ShouldNotHaveAnyValidationErrors();
        }
        else
        {
            if (userType == UserType.Admin)
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? PostMemberReinstateCommandValidator.MemberIdEmptyErrorMessage
                    : errorMessage);

                result.Errors.Count.Should().Be(1);
            }
            else
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? PostMemberReinstateCommandValidator.MemberIdEmptyErrorMessage
                    : errorMessage);
                result.Errors.Count.Should().Be(1);
            }
        }
    }

    [TestCase(MembershipStatusType.Deleted, true)]
    [TestCase(MembershipStatusType.Withdrawn, true)]
    [TestCase(MembershipStatusType.Removed, false)]
    [TestCase(MembershipStatusType.Live, false)]
    public async Task ValidateStatusType(MembershipStatusType status, bool isValid)
    {
        var memberId = Guid.NewGuid();

        Mock<IMembersReadRepository> repositoryMock = new();
        repositoryMock.Setup(r => r.GetMember(It.IsAny<Guid>())).ReturnsAsync(new Member() { Status = status.ToString(), UserType = UserType.Apprentice, IsRegionalChair = true });

        var sut = new PostMemberReinstateCommandValidator(repositoryMock.Object);
        var target = new PostMemberReinstateCommand { MemberId = memberId };
        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveAnyValidationErrors();
        }
        else
        {
            result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(PostMemberReinstateCommandValidator.MemberIdMustBeWithdrawnOrDeleted);

            result.Errors.Count.Should().Be(1);
        }
    }
}
