using AutoFixture.NUnit3;
using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Common.Validators.MemberId;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberActivities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberActivities;
public class GetMemberActivitiesQueryValidatorTests
{
    [Test]
    public async Task ValidateMemberId_Empty_FailsValidation()
    {
        GetMemberActivitiesQuery mockQuery = new GetMemberActivitiesQuery(Guid.Empty);
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();

        var sut = new GetMemberActivitiesQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockQuery);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdEmptyErrorMessage);
    }

    [Test, AutoData]
    public async Task ValidateMemberId_WhenNoMemberReturned_FailsValidation(GetMemberActivitiesQuery mockQuery)
    {
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();
        membersReadRepositoryMock.Setup(m => m.GetMember(It.IsAny<Guid>())).ReturnsAsync(() => null);

        var sut = new GetMemberActivitiesQueryValidator(membersReadRepositoryMock.Object);
        var result = await sut.TestValidateAsync(mockQuery);

        result.ShouldHaveValidationErrorFor(nt => nt.MemberId).WithErrorMessage(MemberIdValidator.MemberIdMustBeLive);
    }
}
