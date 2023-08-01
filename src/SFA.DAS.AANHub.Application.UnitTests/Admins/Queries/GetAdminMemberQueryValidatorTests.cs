using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Admins.Queries;

namespace SFA.DAS.AANHub.Application.UnitTests.Admins.Queries;

public class GetAdminMemberQueryValidatorTests
{
    [TestCase("userName", true)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    public async Task Validates_Admin_UserName_NotNull_Not_Empty(string email, bool isValid)
    {
        var query = new GetAdminMemberQuery(email);
        var sut = new GetAdminMemberQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        else
            result.ShouldHaveValidationErrorFor(c => c.Email);
    }
}