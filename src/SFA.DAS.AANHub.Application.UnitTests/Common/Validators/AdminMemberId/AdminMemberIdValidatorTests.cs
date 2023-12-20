using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.AdminMemberId;
using SFA.DAS.AANHub.Application.Members.Commands.PostMemberRemove;
using SFA.DAS.AANHub.Domain.Common;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Common.Validators.AdminMemberId;
public class AdminMemberIdValidatorTests
{
    private const string Live = "Live";
    private const string NotLive = "NotLive";

    [TestCase(null, UserType.Admin, Live, false, false, AdminMemberIdValidator.RequestedByMemberIdMustNotBeEmpty)]
    [TestCase("00000000-0000-0000-0000-000000000000", UserType.Admin, Live, false, false, AdminMemberIdValidator.RequestedByMemberIdMustNotBeEmpty)]
    [TestCase("ac44a17b-f843-4e1f-979b-aa95c0fe44f2", UserType.Admin, Live, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Admin, Live, false, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, Live, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeAdmin)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, Live, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeAdmin)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Admin, Live, true, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, Live, true, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, Live, true, true)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, NotLive, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, NotLive, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Apprentice, NotLive, true, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Employer, NotLive, true, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    [TestCase("f5521677-7733-4416-b5a7-4c7a231fe469", UserType.Admin, NotLive, false, false, AdminMemberIdValidator.RequestedByMemberIdMustBeLive)]
    public async Task ValidateRequestedByMemberId(string requestedByMemberId, UserType userType, string memberStatus, bool isRegionalChair, bool isValid, string? errorMessage = null)
    {
        var memberId = Guid.NewGuid();

        Mock<IMembersReadRepository> repositoryMock = new();
        repositoryMock.Setup(r => r.GetMember(Guid.Parse("f5521677-7733-4416-b5a7-4c7a231fe469"))).ReturnsAsync(new Member() { Status = memberStatus, UserType = userType, IsRegionalChair = isRegionalChair });
        repositoryMock.Setup(r => r.GetMember(memberId)).ReturnsAsync(new Member() { Status = Domain.Common.Constants.MembershipStatus.Live, UserType = UserType.Apprentice });

        var sut = new AdminMemberIdValidator(repositoryMock.Object);
        var target = new PostMemberRemoveCommand { MemberId = memberId };
        if (requestedByMemberId != null) target.AdminMemberId = Guid.Parse(requestedByMemberId);

        var result = await sut.TestValidateAsync(target);

        if (isValid)
        {
            result.ShouldNotHaveValidationErrorFor(s => s.AdminMemberId);
        }
        else
        {
            result
            .ShouldHaveValidationErrorFor(s => s.AdminMemberId)
            .WithErrorMessage(errorMessage);
        }
    }
}
