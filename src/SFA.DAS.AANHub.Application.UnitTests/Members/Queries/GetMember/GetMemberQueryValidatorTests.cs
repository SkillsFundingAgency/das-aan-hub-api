using FluentValidation.TestHelper;
using Moq;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMember;
using SFA.DAS.AANHub.Domain.Entities;
using SFA.DAS.AANHub.Domain.Interfaces.Repositories;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMember;

public class GetMemberQueryValidatorTests
{
    [TestCase("00000000-0000-0000-0000-000000000000", false)]
    [TestCase("B46C2A2A-E35C-4788-B4B7-1F7E84081846", true)]
    public async Task Validates_MemberId_NotNull(string memberGuid, bool isValid)
    {
        var memberId = Guid.Parse(memberGuid);
        var query = new GetMemberQuery(memberId);
        var member = isValid ? new Member() : null;
        var membersReadRepositoryMock = new Mock<IMembersReadRepository>();

        membersReadRepositoryMock.Setup(m => m.GetMember(memberId)).ReturnsAsync(member);

        var sut = new GetMemberQueryValidator();

        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.MemberId);
        else
            result.ShouldHaveValidationErrorFor(c => c.MemberId)
              .WithErrorMessage(GetMemberQueryValidator.MemberIdMissingMessage);
    }
}
