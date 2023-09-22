using FluentValidation.TestHelper;
using NUnit.Framework;
using SFA.DAS.AANHub.Application.Members.Queries.GetMemberByEmail;

namespace SFA.DAS.AANHub.Application.UnitTests.Members.Queries.GetMemberByEmail;

public class GetMemberByEmailQueryValidatorTests
{
    [TestCase("test@test.com", true)]
    [TestCase(null, false)]
    [TestCase("", false)]
    [TestCase(" ", false)]
    public async Task Validates_Member_Email_NotNull_Not_Empty(string email, bool isValid)
    {
        var query = new GetMemberByEmailQuery(email);
        var sut = new GetMemberByEmailQueryValidator();
        var result = await sut.TestValidateAsync(query);

        if (isValid)
            result.ShouldNotHaveValidationErrorFor(c => c.Email);
        else
        {
            result.ShouldHaveValidationErrorFor(c => c.Email)
                .WithErrorMessage(GetMemberByEmailQueryValidator.EmailMissingMessage);
        }
    }
}
