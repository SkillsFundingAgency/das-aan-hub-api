using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Partners.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.Partners.Queries;

public class GetPartnerMemberQueryValidatorTests
{
    [TestCase("username", true)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    public async Task Validates_UserName_NotNull_NotEmpty(string userName, bool isValid)
    {
        var query = new GetPartnerMemberQuery(userName);
        var sut = new GetPartnerMemberQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.UserName);
        else
            result.ShouldHaveValidationErrorFor(c => c.UserName);
    }
}