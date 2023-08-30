using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Members.Commands.PatchMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using static SFA.DAS.AANHub.Domain.Common.Constants;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Commands.PatchMember.PatchMemberCommandValidatorTests;

public class WhenValidatingMemberId
{
    [TestCase(null, "Apprentice", false)]
    [TestCase("00000000-0000-0000-0000-000000000000", "Apprentice", false)]
    [TestCase("ac44a17b-f843-4e1f-979b-aa95c0fe44f2", "Apprentice", false, MemberIdValidator.MemberIdNotFoundErrorMessage)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", "Apprentice", true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", "Employer", true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", "Admin", false, MemberIdValidator.MemberIdNotFoundErrorMessage)]
    public async Task ThenMemberIdShouldHaveAValidValue(string memberId, string userType, bool isValid, string? errorMessage = null)
    {
        Mock<IMembersReadRepository> repositoryMock = new();
        repositoryMock.Setup(r => r.GetMember(Guid.Parse("f5521677-7733-4416-b5a7-4c7a231fe469"))).ReturnsAsync(new Member() { Status = MembershipStatus.Live, UserType = userType });
        PatchMemberCommandValidator sut = new(repositoryMock.Object);
        PatchMemberCommand target = new();
        if (memberId != null) target.MemberId = Guid.Parse(memberId);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.MemberId);
        }
        else
        {
            if (userType == "Admin")
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? string.Format(MemberIdValidator.MemberIdNotFoundErrorMessage, nameof(PatchMemberCommand.MemberId))
                    : errorMessage);
            }
            else
            {
                result
                .ShouldHaveValidationErrorFor(s => s.MemberId)
                .WithErrorMessage(
                    string.IsNullOrWhiteSpace(errorMessage)
                    ? string.Format(MemberIdValidator.MemberIdEmptyErrorMessage, nameof(PatchMemberCommand.MemberId))
                    : errorMessage);
            }
        }
    }
}
