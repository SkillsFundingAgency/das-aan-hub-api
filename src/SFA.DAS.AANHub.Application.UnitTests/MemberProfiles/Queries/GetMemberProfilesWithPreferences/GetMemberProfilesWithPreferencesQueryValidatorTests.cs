using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AANHub.Application.UnitTests.MemberProfiles.Queries.GetMemberProfilesWithPreferences;
public class GetMemberProfilesWithPreferencesQueryValidatorTests
{
    [Test, MoqAutoData]
    public async Task ValidateMemberId_Empty_FailsValidation(GetMemberProfilesWithPreferencesQuery mockQuery)
    {
        mockQuery.MemberId = Guid.Empty;
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();

        var sut = new GetMemberProfilesWithPreferencesQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockQuery);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdEmptyErrorMessage);
    }

    [Test, MoqAutoData]
    public async Task ValidateMemberId_WhenNoMemberReturned_FailsValidation(GetMemberProfilesWithPreferencesQuery mockQuery)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(() => null);

        var sut = new GetMemberProfilesWithPreferencesQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockQuery);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdMustBeLive);
    }
}
